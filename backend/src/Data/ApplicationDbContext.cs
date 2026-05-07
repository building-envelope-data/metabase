using System;
using Metabase.Data.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SchemaNameOptionsExtension = Metabase.Data.Extensions.SchemaNameOptionsExtension;
using Metabase.Extensions;

namespace Metabase.Data;

// Inspired by
// [Authentication and authorization for SPAs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-3.0)
// [Customize Identity Model](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.0)
public sealed class ApplicationDbContext
: IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
  IDataProtectionKeyContext
{
    private const string DefaultSchemaName = "metabase";
    private readonly string _schemaName;
    private readonly IClock _clock;

    internal const string ComponentCategoryTypeName = "component_category";
    internal const string DatabaseVerificationStateTypeName = "database_verification_state";
    internal const string InstitutionRepresentativeRoleTypeName = "institution_representative_role";
    internal const string InstitutionStateTypeName = "institution_state";
    internal const string InstitutionOperatingStateTypeName = "institution_operating_state";
    internal const string MethodCategoryTypeName = "method_category";
    internal const string PrimeSurfaceTypeName = "prime_surface";
    internal const string StandardizerTypeName = "standardizer";

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IClock clock
    )
        : base(options)
    {
        // The schema-name option is set in `Metabase.Startup` by an invocation
        // of `UseSchemaName` on a `DbContextOptionsBuilder` instance.
        var schemaNameOptions = options.FindExtension<SchemaNameOptionsExtension>();
        _schemaName = schemaNameOptions is null ? DefaultSchemaName : schemaNameOptions.SchemaName;
        _clock = clock;
    }

    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#dbcontext-and-dbset
    public DbSet<Component> Components { get; private set; } = default!;
    public DbSet<ComponentAssembly> ComponentAssemblies { get; private set; } = default!;

    public DbSet<ComponentConcretizationAndGeneralization> ComponentConcretizationAndGeneralizations
    {
        get;
        private set;
    } = default!;

    public DbSet<ComponentManufacturer> ComponentManufacturers { get; private set; } = default!;
    public DbSet<ComponentVariant> ComponentVariants { get; private set; } = default!;
    public DbSet<DataFormat> DataFormats { get; private set; } = default!;
    public DbSet<DataProtectionKey> DataProtectionKeys { get; private set; } = default!;
    public DbSet<Database> Databases { get; private set; } = default!;
    public DbSet<GnuPgKeyFingerprint> GnuPgKeyFingerprints { get; private set; } = default!;
    public DbSet<Institution> Institutions { get; private set; } = default!;
    public DbSet<InstitutionMethodDeveloper> InstitutionMethodDevelopers { get; private set; } = default!;
    public DbSet<InstitutionRepresentative> InstitutionRepresentatives { get; private set; } = default!;
    public DbSet<Method> Methods { get; private set; } = default!;
    public DbSet<OpenIdConnectApplication> OpenIdConnectApplications { get; private set; } = default!;
    public DbSet<OpenIdConnectAuthorization> OpenIdConnectAuthorizations { get; private set; } = default!;
    public DbSet<OpenIdConnectToken> OpenIdConnectTokens { get; private set; } = default!;
    public DbSet<OpenIdConnectScope> OpenIdConnectScopes { get; private set; } = default!;
    public DbSet<UserMethodDeveloper> UserMethodDevelopers { get; private set; } = default!;

    // Inspired by https://github.com/openiddict/openiddict-core/issues/1376#issuecomment-1151275376
    // It is needed to fix the following error that occurred when trying to redeem OpenId Connect tokens in production:
    // `Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported (...)`
    // See also https://github.com/openiddict/openiddict-core/issues/1376
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeUtcValueConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<DateTimeUtcValueConverter>();
        configurationBuilder.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetUtcValueConverter>();
        configurationBuilder.Properties<DateTimeOffset?>().HaveConversion<DateTimeOffsetUtcValueConverter>();
        configurationBuilder.Properties<OffsetDateTime>().HaveConversion<OffsetDateTimeUtcValueConverter>();
        configurationBuilder.Properties<OffsetDateTime?>().HaveConversion<OffsetDateTimeUtcValueConverter>();
        base.ConfigureConventions(configurationBuilder);
    }

    private sealed class DateTimeUtcValueConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeUtcValueConverter()
            : base(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            )
        {
        }
    }

    private sealed class DateTimeOffsetUtcValueConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
    {
        public DateTimeOffsetUtcValueConverter()
            : base(
            v => v.ToUniversalTime(),
            v => v
        )
        {
        }
    }

    private sealed class OffsetDateTimeUtcValueConverter : ValueConverter<OffsetDateTime, OffsetDateTime>
    {
        public OffsetDateTimeUtcValueConverter()
            : base(
            v => v.WithOffset(Offset.Zero),
            v => v
        )
        {
        }
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries<IAuditable>()
            .Where(_ =>
                _.State == EntityState.Added
                || _.State == EntityState.Modified
            // || _.State == EntityState.Deleted
            );
        var now = _clock.GetUtcNow();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    break;
                    // NOTE that soft deletes do not cascade
                    // case EntityState.Deleted:
                    //     // soft delete
                    //     entry.State = EntityState.Modified;
                    //     entry.Entity.DeletedAt = now;
                    //     entry.Entity.UpdatedAt = now;
                    //     break;
            }
        }
    }

    private static void ConfigureIdentityEntities(
        ModelBuilder builder
    )
    {
        // https://stackoverflow.com/questions/19902756/asp-net-identity-dbcontext-confusion/35722688#35722688
        builder.Entity<User>().ToTable("user");
        builder.Entity<Role>().ToTable("role");
        builder.Entity<UserClaim>().ToTable("user_claim");
        builder.Entity<UserRole>().ToTable("user_role");
        builder.Entity<UserLogin>().ToTable("user_login");
        builder.Entity<RoleClaim>().ToTable("role_claim");
        builder.Entity<UserToken>().ToTable("user_token");
    }

    private static void ConfigureComponentAssembly(ModelBuilder builder)
    {
        // https://docs.microsoft.com/en-us/ef/core/modeling/relationships#join-entity-type-configuration
        builder.Entity<Component>()
            .HasMany(c => c.Parts)
            .WithMany(c => c.PartOf)
            .UsingEntity<ComponentAssembly>(
                j => j
                    .HasOne(e => e.PartComponent)
                    .WithMany(c => c.PartOfEdges)
                    .HasForeignKey(e => e.PartComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.AssembledComponent)
                    .WithMany(c => c.PartEdges)
                    .HasForeignKey(e => e.AssembledComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("component_assembly")
                    .HasKey(a => new { a.AssembledComponentId, a.PartComponentId })
            );
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
                    .WithMany(c => c.GeneralizationEdges)
                    .HasForeignKey(e => e.ConcreteComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.GeneralComponent)
                    .WithMany(c => c.ConcretizationEdges)
                    .HasForeignKey(e => e.GeneralComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("component_concretization_and_generalization")
                    .HasKey(a => new { a.GeneralComponentId, a.ConcreteComponentId })
            );
    }

    private static void ConfigureComponentVariant(ModelBuilder builder)
    {
        // https://docs.microsoft.com/en-us/ef/core/modeling/relationships#join-entity-type-configuration
        builder.Entity<Component>()
            .HasMany(c => c.Variants)
            .WithMany(c => c.VariantOf)
            .UsingEntity<ComponentVariant>(
                j => j
                    .HasOne(e => e.ToComponent)
                    .WithMany(c => c.VariantOfEdges)
                    .HasForeignKey(e => e.ToComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.OfComponent)
                    .WithMany(c => c.VariantEdges)
                    .HasForeignKey(e => e.OfComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("component_variant")
                    .HasKey(a => new { a.OfComponentId, a.ToComponentId })
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
                    .HasForeignKey(e => e.InstitutionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.Component)
                    .WithMany(c => c.ManufacturerEdges)
                    .HasForeignKey(e => e.ComponentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("component_manufacturer")
                    .HasKey(a => new { a.ComponentId, a.InstitutionId })
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
                    .HasForeignKey(e => e.InstitutionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.Method)
                    .WithMany(m => m.InstitutionDeveloperEdges)
                    .HasForeignKey(e => e.MethodId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("institution_method_developer")
                    .HasKey(a => new { a.InstitutionId, a.MethodId })
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
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.Institution)
                    .WithMany(i => i.RepresentativeEdges)
                    .HasForeignKey(e => e.InstitutionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("institution_representative")
                    .HasKey(a => new { a.InstitutionId, a.UserId })
            );
    }

    private static void ConfigureOpenIdConnectApplicationOwner(ModelBuilder builder)
    {
        builder.Entity<Institution>()
            .HasMany(i => i.OpenIdConnectApplications)
            .WithOne(a => a.Owner)
            .HasForeignKey(i => i.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureDatabaseOperator(ModelBuilder builder)
    {
        builder.Entity<Institution>()
            .HasMany(i => i.OperatedDatabases)
            .WithOne(i => i.Operator)
            .HasForeignKey(i => i.OperatorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureUserMethodDeveloper(ModelBuilder builder)
    {
        builder.Entity<Method>()
            .HasMany(m => m.UserDevelopers)
            .WithMany(i => i.DevelopedMethods)
            .UsingEntity<UserMethodDeveloper>(
                j => j
                    .HasOne(e => e.User)
                    .WithMany(i => i.DevelopedMethodEdges)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(e => e.Method)
                    .WithMany(m => m.UserDeveloperEdges)
                    .HasForeignKey(e => e.MethodId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .ToTable("user_method_developer")
                    .HasKey(a => new { a.UserId, a.MethodId })
            );
    }

    private static void ConfigureInstitutionManager(ModelBuilder builder)
    {
        builder.Entity<Institution>()
            .HasMany(i => i.ManagedInstitutions)
            .WithOne(i => i.Manager)
            .HasForeignKey(i => i.ManagerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureComponentManager(ModelBuilder builder)
    {
        builder.Entity<Institution>()
            .HasMany(i => i.ManagedComponents)
            .WithOne(i => i.Manager)
            .HasForeignKey(i => i.ManagerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureDataFormatManager(ModelBuilder builder)
    {
        builder.Entity<Institution>()
            .HasMany(i => i.ManagedDataFormats)
            .WithOne(i => i.Manager)
            .HasForeignKey(i => i.ManagerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureMethodManager(ModelBuilder builder)
    {
        builder.Entity<Institution>()
            .HasMany(i => i.ManagedMethods)
            .WithOne(i => i.Manager)
            .HasForeignKey(i => i.ManagerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureOpenIdConnect(ModelBuilder builder)
    {
        // auto-include for GraphQL `OpenIdConnectAuthorizationType`
        builder.Entity<OpenIdConnectAuthorization>()
            .Navigation(a => a.Application)
            .AutoInclude();
        // auto-include for GraphQL `OpenIdConnectTokenType`
        builder.Entity<OpenIdConnectToken>()
            .Navigation(a => a.Application)
            .AutoInclude();
        builder.Entity<OpenIdConnectToken>()
            .Navigation(a => a.Authorization)
            .AutoInclude();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(_schemaName);
        builder.HasPostgresExtension("pgcrypto"); // https://www.npgsql.org/efcore/modeling/generated-properties.html#guiduuid-generation
        builder.Entity<Component>().ToTable("component");
        builder.Entity<Database>().ToTable("database");
        builder.Entity<GnuPgKeyFingerprint>().ToTable("gnu_pg_fingerprint");
        builder.Entity<DataFormat>().ToTable("data_format");
        builder.Entity<Institution>().ToTable("institution");
        builder.Entity<Method>().ToTable("method");
        ConfigureIdentityEntities(builder);
        ConfigureComponentAssembly(builder);
        ConfigureComponentConcretizationAndGeneralization(builder);
        ConfigureComponentManufacturer(builder);
        ConfigureComponentVariant(builder);
        ConfigureInstitutionMethodDeveloper(builder);
        ConfigureInstitutionRepresentative(builder);
        ConfigureOpenIdConnectApplicationOwner(builder);
        ConfigureDatabaseOperator(builder);
        ConfigureUserMethodDeveloper(builder);
        ConfigureInstitutionManager(builder);
        ConfigureComponentManager(builder);
        ConfigureDataFormatManager(builder);
        ConfigureMethodManager(builder);
        ConfigureOpenIdConnect(builder);
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(IEntity).IsAssignableFrom(entityType.ClrType))
            {
                var entity = builder.Entity(entityType.ClrType);
                // https://www.npgsql.org/efcore/modeling/generated-properties.html#guiduuid-generation
                entity
                    .Property(nameof(IEntity.Id))
                    .HasDefaultValueSql("gen_random_uuid()");
                // https://www.npgsql.org/efcore/modeling/concurrency.html#the-postgresql-xmin-system-column
                entity
                    .Property(nameof(IEntity.Version))
                    .IsRowVersion();
            }
            if (typeof(IAssociation).IsAssignableFrom(entityType.ClrType))
            {
                var association = builder.Entity(entityType.ClrType);
                // https://www.npgsql.org/efcore/modeling/concurrency.html#the-postgresql-xmin-system-column
                association
                    .Property(nameof(IAssociation.Version))
                    .IsRowVersion();
            }
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var auditable = builder.Entity(entityType.ClrType);
                auditable
                    .Property(nameof(IAuditable.CreatedAt))
                    .HasDefaultValueSql("now()");
                auditable
                    .Property(nameof(IAuditable.UpdatedAt))
                    .HasDefaultValueSql("now()");
                // exclude soft-deleted entities with the effect that
                // `context.<Auditables>.ToList()` only returns rows where
                // `DeletedAt` is null and
                // `context.<Auditables>.IgnoreQueryFilters().ToList()` returns
                // all rows
                // entity
                //     .HasQueryFilter((IAuditable _) => _.DeletedAt == null);
            }
            if (typeof(IEntity).IsAssignableFrom(entityType.ClrType)
                && typeof(INamed).IsAssignableFrom(entityType.ClrType))
            {
                var entity = builder.Entity(entityType.ClrType);
                // https://www.npgsql.org/efcore/modeling/generated-properties.html#guiduuid-generation
                entity
                    .HasIndex(nameof(INamed.Name), nameof(IEntity.Id))
                    .IsUnique();
            }
        }
    }
}