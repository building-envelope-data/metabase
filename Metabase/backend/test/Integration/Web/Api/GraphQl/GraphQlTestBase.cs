namespace Test.Integration.Web.Api.GraphQl
{
    public abstract class GraphQlTestBase : TestBase
    {
        protected GraphQlClient Client { get; }

        protected GraphQlTestBase(CustomWebApplicationFactory factory)
          : base(factory)
        {
            Client = new GraphQlClient(HttpClient);
        }
    }
}