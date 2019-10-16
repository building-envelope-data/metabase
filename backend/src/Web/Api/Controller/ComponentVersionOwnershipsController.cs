using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icon.Data;
using Microsoft.AspNetCore.Authorization;
using ComponentVersionOwnershipAggregate = Icon.Domain.ComponentVersionOwnershipAggregate;
using ComponentVersionOwnership = Icon.Domain.ComponentVersionOwnership;
using User = Icon.Domain.User;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using static IdentityServer4.IdentityServerConstants;
/* using Microsoft.AspNetCore.Identity; */
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Icon.Web.Api.Controller
{
    [Route("api/components/{componentId:Guid}/versions/{versionId:Guid}/ownerships")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(LocalApi.PolicyName)]
    public class ComponentVersionOwnershipsController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly UserManager<User> _userManager;

        public ComponentVersionOwnershipsController(ICommandBus commandBus, IQueryBus queryBus, UserManager<User> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }

        /* [HttpGet] */
        /* [AllowAnonymous] */
        /* public async Task<ActionResult<IEnumerable<OwnershipAggregate>>> List() */
        /* { */
        /*     var ownerships = await _queryBus.Send<ComponentVersionOwnership.List.Query, IEnumerable<OwnershipAggregate>>( */
        /*         new ComponentVersionOwnership.List.Query() */
        /*         ); */
        /*     return Ok(ownerships); */
        /* } */

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ComponentVersionOwnershipAggregate>> Get([FromRoute] Guid id)
        {
            return NotFound();
            /* var componentVersion = await _dbContext.ComponentVersions.FindAsync(id); */

            /* if (componentVersion == null) */
            /* { */
            /*     return NotFound(); */
            /* } */

            /* return componentVersion; */
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromRoute] Guid componentId, [FromRoute] Guid versionId, [FromBody] ComponentVersionOwnership.Create.Data data)
        {
            var user = await _userManager.GetUserAsync(User);
            var ownership = await _commandBus.Send<ComponentVersionOwnership.Create.Command, ComponentVersionOwnershipAggregate>(
                ComponentVersionOwnership.Create.Command.From(versionId, data, user.Id));
            return CreatedAtAction(
                nameof(Get),
                new
                {
                    componentId = componentId,
                    versionId = versionId,
                    id = ownership.Id
                },
                ownership.Id
                );
        }
    }
}