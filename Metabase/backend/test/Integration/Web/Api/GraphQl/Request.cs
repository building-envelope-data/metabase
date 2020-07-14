using System.Collections.Generic;

namespace Test.Integration.Web.Api.GraphQl
{
    public class Request
    {
        public string query { get; }
        public string? operationName { get; }
        public Dictionary<string, object?>? variables { get; }

        public Request(
            string query,
            string? operationName,
            Dictionary<string, object?>? variables
            )
        {
            this.query = query;
            this.operationName = operationName;
            this.variables = variables;
        }
    }
}