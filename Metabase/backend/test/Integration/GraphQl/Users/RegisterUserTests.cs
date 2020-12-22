/* using System; */
/* using System.Threading.Tasks; */
/* using HotChocolate; */
/* using HotChocolate.Execution; */
/* using Microsoft.EntityFrameworkCore; */
/* using Microsoft.Extensions.DependencyInjection; */
/* using Snapshooter.Xunit; */
/* using Xunit; */

/* namespace Metabase.Tests.GraphQl.Users */
/* { */
/*     public sealed class RegisterUserTests */
/*     { */
/*         [Fact] */
/*         public async Task RegisterUser() */
/*         { */
/*             // arrange */
/*             IServiceProvider services = new ServiceCollection() */
/*                 .AddDbContextPool<ApplicationDbContext>( */
/*                     options => options.UseInMemoryDatabase("Data Source=conferences.db")) */
/*                 .AddGraphQl() */
/*                     .AddQueryType(d => d.Name("Query")) */
/*                         .AddType<UserQueries>() */
/*                     .AddMutationType(d => d.Name("Mutation")) */
/*                         .AddType<UserMutations>() */
/*                     .AddType<UserType>() */
/*                     .AddType<SessionType>() */
/*                     .AddType<SpeakerType>() */
/*                     .AddType<TrackType>() */
/*                     // .EnableRelaySupport() */
/*                 .Services */
/*                 .BuildServiceProvider(); */

/*             // act */
/*             IExecutionResult result = await services.ExecuteRequestAsync( */
/*                 QueryRequestBuilder.New() */
/*                     .SetQuery(@" */
/*                         mutation RegisterUser { */
/*                             registerUser( */
/*                                 input: { */
/*                                     emailAddress: ""michael@chillicream.com"" */
/*                                         firstName: ""michael"" */
/*                                         lastName: ""staib"" */
/*                                         userName: ""michael3"" */
/*                                     }) */ 
/*                             { */
/*                                 user { */
/*                                     id */
/*                                 } */
/*                             } */
/*                         }") */
/*                     .Create()); */
            
/*             // assert */
/*             result.MatchSnapshot(); */
/*         } */
/*     } */
/* } */
