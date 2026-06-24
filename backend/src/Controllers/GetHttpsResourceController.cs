using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authentication;
using Metabase.Data;
using Metabase.GraphQl.DataX;
using Metabase.GraphQl.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Controllers;

// Inspired by https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0#upload-large-files-with-streaming
// and https://github.com/dotnet/AspNetCore.Docs/blob/b4599432690b8753fc2eac23d52957f47e01997a/aspnetcore/mvc/models/file-uploads/samples/3.x/SampleApp/
[ApiController]
public sealed class GetHttpsResourcesController : Controller
{
    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!_disposed)
        {
            // Dispose of resources held by this instance.
            _disposed = true;
        }
    }

    // Disposable types implement a finalizer.
    ~GetHttpsResourcesController()
    {
        Dispose(false);
    }

    private const string GetByVertexIdRouteName = "GetResourceByVertexId";

    [HttpGet("~/api/resources/{vertexId}", Name = GetByVertexIdRouteName)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.IdentityAndCookieAndBearerTokenAuthenticationScheme)]
    [AllowAnonymous]
    [EndpointDescription("Get an HTTP resource in the media type of its data format from a database passing along an access token that identifies the logged-in user.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string vertexId,
        [FromQuery] Guid dataId,
        [FromQuery] DataKind dataKind,
        [FromQuery] Guid databaseId,
        [FromServices] IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        [FromServices] QueryingDatabases queryingDatabases,
        [FromServices] DataQueries dataQueries,
        CancellationToken cancellationToken
    )
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        var database = await databaseContext.Databases.AsNoTracking()
            .Where(_ => _.Id == databaseId)
            .SingleOrDefaultAsync(cancellationToken);
        if (database is null)
        {
            return Problem(
                title: "Database Not Found",
                detail: $"There is no database with ID '{databaseId:D}'.",
                statusCode: StatusCodes.Status404NotFound,
                instance: HttpContext.Request.Path
            );
        }
        var data = await dataQueries.GetDataAsync(database, dataId, dataKind, null, null, cancellationToken);
        if (data is null)
        {
            return Problem(
                title: "Data Not Found",
                detail: $"There is no data with ID '{dataId:D}' of kind '{dataKind}' in the database with ID '{databaseId:D}'.",
                statusCode: StatusCodes.Status404NotFound,
                instance: HttpContext.Request.Path
            );
        }
        var getHttpsResource =
            data.ResourceTree.Root.VertexId == vertexId
            ? data.ResourceTree.Root.Value
            : data.ResourceTree.NonRootVertices.FirstOrDefault(_ => _.VertexId == vertexId)?.Value;
        if (getHttpsResource is null)
        {
            return Problem(
                title: "Resource Not Found",
                detail: $"There is no GET HTTPS resource with vertex ID '{vertexId}' in the data with ID '{dataId:D}' of kind '{dataKind}' in the database with ID '{databaseId:D}'.",
                statusCode: StatusCodes.Status404NotFound,
                instance: HttpContext.Request.Path
            );
        }
        var dataFormat = await databaseContext.DataFormats.AsNoTracking()
            .Where(_ => _.Id == getHttpsResource.DataFormatId)
            .SingleOrDefaultAsync(cancellationToken);
        if (database is null)
        {
            return Problem(
                title: "Database Not Found",
                detail: $"There is no database with ID '{databaseId:D}'.",
                statusCode: StatusCodes.Status404NotFound,
                instance: HttpContext.Request.Path
            );
        }
        // Do not use `using var ...` below to not dispose objects when the associated response stream is still in use
        var httpClient = await queryingDatabases.CreateHttpClientAsync(database, cancellationToken);
        var httpResponseMessage =
            await httpClient.GetAsync(
                getHttpsResource.Locator,
                HttpCompletionOption.ResponseHeadersRead, // do not buffer in memory before returning
                cancellationToken
            );
        if (httpResponseMessage.StatusCode is not HttpStatusCode.OK)
        {
            return Problem(
                title: "Fetch Failure",
                detail: $"Failed to fetch the GET HTTPS resource from '{getHttpsResource.Locator}'.",
                statusCode: (int)httpResponseMessage.StatusCode,
                instance: HttpContext.Request.Path
            );
        }
        var contentType = httpResponseMessage.Content.Headers.ContentType;
        var responseStream =
            await httpResponseMessage.Content
                .ReadAsStreamAsync(cancellationToken);
        var fileExtension = dataFormat?.Extension is null ? "unknown" : dataFormat.Extension;
        return File(
            responseStream,
            contentType?.ToString() ?? MediaTypeNames.Application.Octet,
            $"{getHttpsResource.HashValue}.{fileExtension}",
            enableRangeProcessing: true
        );
    }
}