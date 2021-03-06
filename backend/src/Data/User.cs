using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

// TODO Make `User`, `Role`, ... subtype `Entity` and use `xmin` to catch update conflicts. Add interface `IEntity`.
namespace Metabase.Data
{
    public sealed class User
        : IdentityUser<Guid>,
          Data.IEntity,
          IStakeholder
    {
        [GraphQLDescription("Full name")]
        public string Name { get; private set; }

        [MinLength(1)]
        public string? PostalAddress { get; private set; }

        [Url]
        public Uri? WebsiteLocator { get; private set; }

        // public ICollection<UserClaim> Claims { get; } = new List<UserClaim>();
        // public ICollection<UserLogin> Logins { get; } = new List<UserLogin>();
        // public ICollection<UserToken> Tokens { get; } = new List<UserToken>();
        // public ICollection<UserRole> Roles { get; } = new List<UserRole>();

        public ICollection<UserMethodDeveloper> DevelopedMethodEdges { get; } = new List<UserMethodDeveloper>();
        public ICollection<Method> DevelopedMethods { get; } = new List<Method>();

        public ICollection<InstitutionRepresentative> RepresentedInstitutionEdges { get; } = new List<InstitutionRepresentative>();

        public ICollection<Institution> RepresentedInstitutions { get; } = new List<Institution>();

        public uint xmin { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html

#nullable disable
        public User()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public User(
            string name,
            string email,
            string? postalAddress,
            Uri? websiteLocator
            )
        {
            Email = email;
            UserName = email; // TODO Make `UserName`, `ConfirmedUserName`, ... of `IdentityUser` aliases for the respective `*Email` properties!
            Name = name;
            PostalAddress = postalAddress;
            WebsiteLocator = websiteLocator;
        }
    }
}