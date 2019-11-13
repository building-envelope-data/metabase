using System.Collections.Generic;

namespace Test.Integration.Web.Api.GraphQl
{
    public class Request
    {
        public string query { get; set; }
        public string operationName { get; set; }
        public Dictionary<string, object> variables { get; set; }
    }
}