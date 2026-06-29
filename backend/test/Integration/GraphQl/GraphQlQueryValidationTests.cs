// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Threading.Tasks;
// using HotChocolate;
// using HotChocolate.Validation;
// using HotChocolate.Language;
// using Microsoft.Extensions.DependencyInjection;
// using NUnit.Framework;
// using FluentAssertions;
// using System.Diagnostics.CodeAnalysis;
//
// namespace Metabase.Tests.Integration.GraphQl;
//
// [TestFixture]
// public class GraphQlQueryValidationTests
// {
//     private static readonly string MetabaseSchemaPath = System.IO.Path.GetFullPath(
//         System.IO.Path.Combine("..", "..", "..", "Integration", "GraphQl", "__snapshots__", "GraphQlSchemaTests.IsUnchanged.snap")
//     );
//
//     private static readonly string DatabaseSchemaPath = System.IO.Path.GetFullPath(
//         System.IO.Path.Combine("GraphQl", "Databases", "Queries", "database.graphqls")
//     );
//
//     private static readonly string DatabaseGraphQlQueryFolder = System.IO.Path.GetFullPath(
//         System.IO.Path.Combine("src", "GraphQl", "Databases", "Queries")
//     );
//
//     // Code Smell: The variables are properly set in `OneTimeSetUp`
//     private (Schema, DocumentValidator) metabaseValidator = default!;
//     private (Schema, DocumentValidator) databaseValidator = default!;
//
//     [OneTimeSetUp]
//     public async Task OneTimeSetUp()
//     {
//         metabaseValidator = await CreateValidator(MetabaseSchemaPath);
//         databaseValidator = await CreateValidator(DatabaseSchemaPath);
//     }
//
//     private static async Task<(Schema, DocumentValidator)> CreateValidator(string schemaPath)
//     {
//         if (!File.Exists(schemaPath))
//         {
//             Assert.Fail($"GraphQL schema file not found at: {schemaPath}");
//         }
//         var schema = SchemaBuilder.New()
//             .AddDocumentFromString(await File.ReadAllTextAsync(schemaPath))
//             .Use(_ => _)
//             .Create();
//         var validator = new ServiceCollection()
//             .AddValidation()
//             .BuildServiceProvider()
//             .GetRequiredService<DocumentValidatorBuilder>()
//             .Build();
//         return (schema, validator);
//     }
//
//     [Test]
//     [TestCaseSource(nameof(GetMetabaseGraphQLQueryFiles))]
//     [SuppressMessage("Naming", "CA1707")]
//     public Task QueryFile_MatchesMetabaseSchema(string queryFilePath)
//     {
//         return AssertQueryFileMatchesSchema(queryFilePath, metabaseValidator);
//     }
//
//     [Test]
//     [TestCaseSource(nameof(GetDatabaseGraphQLQueryFiles))]
//     [SuppressMessage("Naming", "CA1707")]
//     public Task QueryFile_MatchesDatabaseSchema(string queryFilePath)
//     {
//         return AssertQueryFileMatchesSchema(queryFilePath, databaseValidator);
//     }
//
//     private static IEnumerable<string> GetMetabaseGraphQLQueryFiles()
//     {
//         var queriesFolderPath = System.IO.Path.GetFullPath(".");
//         return Directory.GetFiles(queriesFolderPath, "*.graphql", SearchOption.AllDirectories)
//             .Where(filePath => !filePath.Contains(DatabaseGraphQlQueryFolder, StringComparison.OrdinalIgnoreCase));
//     }
//
//     private static string[] GetDatabaseGraphQLQueryFiles()
//     {
//         return Directory.GetFiles(DatabaseGraphQlQueryFolder, "*.graphql", SearchOption.AllDirectories);
//     }
//
//     public static async Task AssertQueryFileMatchesSchema(string queryFilePath, (Schema schema, DocumentValidator validator) schemaAndValidator)
//     {
//         var queryText = await File.ReadAllTextAsync(queryFilePath);
//         DocumentNode queryDocument;
//         try
//         {
//             queryDocument = Utf8GraphQLParser.Parse(queryText);
//         }
//         catch (SyntaxException exception)
//         {
//             Assert.Fail($"GraphQL Syntax Error in file '{System.IO.Path.GetFileName(queryFilePath)}': {exception.Message}");
//             return;
//         }
//         var validationResult = schemaAndValidator.validator.Validate(schemaAndValidator.schema, queryDocument);
//         if (validationResult.HasErrors)
//         {
//             var formattedErrors = validationResult.Errors.Select(error =>
//                 $"[{error.Code}] {error.Message} (Line: {(error.Locations?.Count > 0 ? error.Locations[0].Line : -1)}, Column: {(error.Locations?.Count > 0 ? error.Locations[0].Column : -1)})"
//             );
//             validationResult.Errors.Should().BeEmpty($"""
//                 because the GraphQL query file '{queryFilePath}' must comply with the schema definition. Found issues:
//                 {string.Join(Environment.NewLine, formattedErrors)}
//             """);
//         }
//     }
// }