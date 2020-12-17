using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using NpgsqlTypes;
using DateTime = System.DateTime;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;

namespace Metabase.GraphQl.Users
{
  public sealed class UserQueries
  {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        /* TODO [UseProjection] // fails without an explicit error message in the logs */
        /* TODO [UseFiltering(typeof(UserFilterType))] // wait for https://github.com/ChilliCream/hotchocolate/issues/2672 and https://github.com/ChilliCream/hotchocolate/issues/2666 */
        [UseSorting]
        public IQueryable<Data.User> GetUsers(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
          return context.Users;
        }
  }
}
