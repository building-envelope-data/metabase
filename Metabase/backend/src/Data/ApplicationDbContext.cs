using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql;
using SchemaNameOptionsExtension = Infrastructure.Data.SchemaNameOptionsExtension;

namespace Metabase.Data
{
    // Inspired by
    // [Authentication and authorization for SPAs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-3.0)
    // [Customize Identity Model](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.0)
    public sealed class ApplicationDbContext
      : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        static ApplicationDbContext()
        {
            RegisterEnumerations();
        }

        private static void RegisterEnumerations()
        {
            // https://www.npgsql.org/efcore/mapping/enum.html#mapping-your-enum
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Enumerations.ComponentCategory>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Enumerations.InstitutionRepresentativeRole>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Enumerations.InstitutionState>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Enumerations.MethodCategory>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Enumerations.Standardizer>();
        }

        private static void CreateEnumerations(ModelBuilder builder)
        {
            // https://www.npgsql.org/efcore/mapping/enum.html#creating-your-database-enum
            // Create enumerations in public schema because that is where `NpgsqlConnection.GlobalTypeMapper.MapEnum` expects them to be by default.
            builder.HasPostgresEnum<Enumerations.ComponentCategory>("public");
            builder.HasPostgresEnum<Enumerations.InstitutionRepresentativeRole>("public");
            builder.HasPostgresEnum<Enumerations.InstitutionState>("public");
            builder.HasPostgresEnum<Enumerations.MethodCategory>("public");
            builder.HasPostgresEnum<Enumerations.Standardizer>("public");
        }

        private static
          EntityTypeBuilder<TEntity>
          ConfigureEntity<TEntity>(
            EntityTypeBuilder<TEntity> builder
            )
          where TEntity : Infrastructure.Data.Entity
        {
            // https://www.npgsql.org/efcore/modeling/generated-properties.html#guiduuid-generation
            builder
              .Property(e => e.Id)
              .HasDefaultValueSql("gen_random_uuid()");
            // https://www.npgsql.org/efcore/modeling/concurrency.html#the-postgresql-xmin-system-column
            builder
              .UseXminAsConcurrencyToken();
            return builder;
        }

        private static void ConfigureIdentityEntities(
            ModelBuilder builder
            )
        {
            // https://stackoverflow.com/questions/19902756/asp-net-identity-dbcontext-confusion/35722688#35722688
            builder.Entity<User>()
              .UseXminAsConcurrencyToken()
              .ToTable("user");
            builder.Entity<Role>().ToTable("role");
            builder.Entity<UserClaim>().ToTable("user_claim");
            builder.Entity<UserRole>().ToTable("user_role");
            builder.Entity<UserLogin>().ToTable("user_login");
            builder.Entity<RoleClaim>().ToTable("role_claim");
            builder.Entity<UserToken>().ToTable("user_token");
        }

        private static void ConfigureComponentConcretizationAndGeneralization(ModelBuilder builder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/relationships#join-entity-type-configuration
            builder.Entity<Component>()
              .HasMany(c => c.Concretizations)
              .WithMany(c => c.Generalizations)
              .UsingEntity<ComponentConcretizationAndGeneralization>(
                j => j
                .HasOne(e => e.ConcreteComponent)
                .WithMany(c => c.ConcretizationEdges)
                .HasForeignKey(e => e.ConcreteComponentId),
                j => j
                .HasOne(e => e.GeneralComponent)
                .WithMany(c => c.GeneralizationEdges)
                .HasForeignKey(e => e.GeneralComponentId),
                j => j
                .ToTable("component_concretization_and_generalization")
                .HasKey(a => new { a.GeneralComponentId, a.ConcreteComponentId })
              );
        }

        private static void ConfigureComponentManufacturer(ModelBuilder builder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/relationships#join-entity-type-configuration
            builder.Entity<Component>()
              .HasMany(c => c.Manufacturers)
              .WithMany(i => i.ManufacturedComponents)
              .UsingEntity<ComponentManufacturer>(
                j => j
                .HasOne(e => e.Institution)
                .WithMany(i => i.ManufacturedComponentEdges)
                .HasForeignKey(e => e.InstitutionId),
                j => j
                .HasOne(e => e.Component)
                .WithMany(c => c.ManufacturerEdges)
                .HasForeignKey(e => e.ComponentId),
                j => j
                .ToTable("component_manufacturer")
                .HasKey(a => new { a.ComponentId, a.InstitutionId })
              );
        }

        private static void ConfigureInstitutionRepresentative(ModelBuilder builder)
        {
            builder.Entity<Institution>()
              .HasMany(i => i.Representatives)
              .WithMany(u => u.RepresentedInstitutions)
              .UsingEntity<InstitutionRepresentative>(
                j => j
                .HasOne(e => e.User)
                .WithMany(u => u.RepresentedInstitutionEdges)
                .HasForeignKey(e => e.UserId),
                j => j
                .HasOne(e => e.Institution)
                .WithMany(i => i.RepresentativeEdges)
                .HasForeignKey(e => e.InstitutionId),
                j => j
                .ToTable("institution_representative")
                .HasKey(a => new { a.InstitutionId, a.UserId })
              );
        }

        private static void ConfigureInstitutionMethodDeveloper(ModelBuilder builder)
        {
            builder.Entity<Method>()
              .HasMany(m => m.InstitutionDevelopers)
              .WithMany(i => i.DevelopedMethods)
              .UsingEntity<InstitutionMethodDeveloper>(
                j => j
                .HasOne(e => e.Institution)
                .WithMany(i => i.DevelopedMethodEdges)
                .HasForeignKey(e => e.InstitutionId),
                j => j
                .HasOne(e => e.Method)
                .WithMany(m => m.InstitutionDeveloperEdges)
                .HasForeignKey(e => e.MethodId),
                j => j
                .ToTable("institution_method_developer")
                .HasKey(a => new { a.InstitutionId, a.MethodId })
              );
        }

        private static void ConfigurePersonAffiliation(ModelBuilder builder)
        {
            builder.Entity<Person>()
              .HasMany(p => p.AffiliatedInstitutions)
              .WithMany(i => i.AffiliatedPersons)
              .UsingEntity<PersonAffiliation>(
                j => j
                .HasOne(e => e.Institution)
                .WithMany(i => i.AffiliatedPersonEdges)
                .HasForeignKey(e => e.InstitutionId),
                j => j
                .HasOne(e => e.Person)
                .WithMany(p => p.AffiliatedInstitutionEdges)
                .HasForeignKey(e => e.PersonId),
                j => j
                .ToTable("person_affiliation")
                .HasKey(a => new { a.PersonId, a.InstitutionId })
              );
        }

        private static void ConfigurePersonMethodDeveloper(ModelBuilder builder)
        {
            builder.Entity<Method>()
              .HasMany(m => m.PersonDevelopers)
              .WithMany(i => i.DevelopedMethods)
              .UsingEntity<PersonMethodDeveloper>(
                j => j
                .HasOne(e => e.Person)
                .WithMany(i => i.DevelopedMethodEdges)
                .HasForeignKey(e => e.PersonId),
                j => j
                .HasOne(e => e.Method)
                .WithMany(m => m.PersonDeveloperEdges)
                .HasForeignKey(e => e.MethodId),
                j => j
                .ToTable("person_method_developer")
                .HasKey(a => new { a.PersonId, a.MethodId })
              );
        }

        private readonly string _schemaName;

        // https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#dbcontext-and-dbset
        public DbSet<Component> Components { get; private set; } = default!;
        public DbSet<ComponentConcretizationAndGeneralization> ComponentConcretizationAndGeneralizations { get; private set; } = default!;
        public DbSet<ComponentManufacturer> ComponentManufacturers { get; private set; } = default!;
        public DbSet<Database> Databases { get; private set; } = default!;
        public DbSet<Institution> Institutions { get; private set; } = default!;
        public DbSet<InstitutionRepresentative> InstitutionRepresentatives { get; private set; } = default!;
        public DbSet<Method> Methods { get; private set; } = default!;
        public DbSet<Person> Persons { get; private set; } = default!;
        public DbSet<PersonAffiliation> PersonAffiliations { get; private set; } = default!;
        public DbSet<Standard> Standards { get; private set; } = default!;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
            )
          : base(options)
        {
            var schemaNameOptions = options.FindExtension<SchemaNameOptionsExtension>();
            _schemaName = schemaNameOptions is null ? "metabase" : schemaNameOptions.SchemaName;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(_schemaName);
            builder.HasPostgresExtension("pgcrypto"); // https://www.npgsql.org/efcore/modeling/generated-properties.html#guiduuid-generation
            CreateEnumerations(builder);
            ConfigureIdentityEntities(builder);
            ConfigureEntity(
                builder.Entity<Component>()
                )
              .ToTable("component");
            ConfigureComponentConcretizationAndGeneralization(builder);
            ConfigureComponentManufacturer(builder);
            ConfigureEntity(
                builder.Entity<Database>()
                )
              .ToTable("database");
            ConfigureEntity(
                builder.Entity<Institution>()
                )
              .ToTable("institution");
            ConfigureInstitutionMethodDeveloper(builder);
            ConfigureInstitutionRepresentative(builder);
            ConfigureEntity(
                builder.Entity<Method>()
                )
              .ToTable("method");
            ConfigureEntity(
                builder.Entity<Person>()
                )
              .ToTable("person");
            ConfigurePersonAffiliation(builder);
            ConfigurePersonMethodDeveloper(builder);
            ConfigureEntity(
                builder.Entity<Standard>()
                )
              .ToTable("standard");
        }
    }
}