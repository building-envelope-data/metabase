using Guid = System.Guid;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public class ListComponents :
      IQuery<IEnumerable<Models.Component>>
    {
    }
}
