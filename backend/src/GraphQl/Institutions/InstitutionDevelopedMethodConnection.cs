using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionDevelopedMethodConnection
        : Connection<Data.Institution, Data.InstitutionMethodDeveloper, InstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge>
    {
        public InstitutionDevelopedMethodConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionDevelopedMethodEdge(x)
                )
        {
        }
    }
}