using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icon.Data;
using Microsoft.AspNetCore.Authorization;
using ComponentVersionAggregate = Icon.Domain.ComponentVersionAggregate;
using ComponentVersion = Icon.Domain.ComponentVersion;
using User = Icon.Domain.User;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using static IdentityServer4.IdentityServerConstants;
/* using Microsoft.AspNetCore.Identity; */
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Icon.Web.Api.Controller
{
    [Route("api/components/{componentId:Guid}/versions")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(LocalApi.PolicyName)]
    public class ComponentVersionsController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly UserManager<User> _userManager;

        public ComponentVersionsController(ICommandBus commandBus, IQueryBus queryBus, UserManager<User> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }

        /* [HttpGet] */
        /* [AllowAnonymous] */
        /* public async Task<ActionResult<IEnumerable<ComponentVersionAggregate>>> List() */
        /* { */
        /*     var componentVersions = await _queryBus.Send<ComponentVersion.List.Query, IEnumerable<ComponentVersionAggregate>>(new ComponentVersion.List.Query()); */
        /*     return Ok(componentVersions); */
        /* } */

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ComponentVersionAggregate>> Get([FromRoute] Guid id)
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
        public async Task<ActionResult<Guid>> Post([FromRoute] Guid componentId)
        {
            var user = await _userManager.GetUserAsync(User);
            var componentVersion = await _commandBus.Send<ComponentVersion.Create.Command, ComponentVersionAggregate>(
                new ComponentVersion.Create.Command(creatorId: user.Id)
                {
                    ComponentId = componentId,
                }
                );
            return CreatedAtAction(
                nameof(Get),
                new
                {
                    componentId = componentId,
                    id = componentVersion.Id
                },
                componentVersion.Id
                );
        }
    }
}