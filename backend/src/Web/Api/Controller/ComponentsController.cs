using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icon.Data;
using Microsoft.AspNetCore.Authorization;
using ComponentAggregate = Icon.Domain.ComponentAggregate;
using ComponentView = Icon.Domain.ComponentView;
using Component = Icon.Domain.Component;
using User = Icon.Domain.User;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using static IdentityServer4.IdentityServerConstants;
/* using Microsoft.AspNetCore.Identity; */
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */

namespace Icon.Web.Api.Controller
{
    [Route("api/components")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(LocalApi.PolicyName)]
    public class ComponentsController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly UserManager<User> _userManager;

        public ComponentsController(ICommandBus commandBus, IQueryBus queryBus, UserManager<User> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ComponentAggregate>>> List()
        {
            var components = await _queryBus.Send<Component.List.Query, IEnumerable<ComponentAggregate>>(
                new Component.List.Query()
                );
            return Ok(components);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ComponentView>> Get([FromRoute] Guid id, [FromQuery] DateTime timestamp) // TODO Use `ZonedDateTime` here. Problem: Its (de)serialization is rather complex.
        {
            var component = await _queryBus.Send<Component.Get.Query, ComponentView>(
                new Component.Get.Query()
                {
                    ComponentId = id,
                    Timestamp = timestamp,
                }
                );
            if (component == null)
            {
                return NotFound();
            }
            return component;
        }

        // TODO Use PATCH for partial updates!

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /* [HttpPut("{id}")] */
        /* public async Task<IActionResult> PutComponent([FromRoute] Guid id, [FromBody] Component component) */
        /* { */
        /*     if (id != component.Id) */
        /*     { */
        /*         return BadRequest(); */
        /*     } */

        /*     _dbContext.Entry(component).State = EntityState.Modified; */

        /*     try */
        /*     { */
        /*         await _dbContext.SaveChangesAsync(); */
        /*     } */
        /*     catch (DbUpdateConcurrencyException) */
        /*     { */
        /*         if (!ComponentExists(id)) */
        /*         { */
        /*             return NotFound(); */
        /*         } */
        /*         else */
        /*         { */
        /*             throw; */
        /*         } */
        /*     } */

        /*     return NoContent(); */
        /* } */

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Guid>> Post()
        {
            var user = await _userManager.GetUserAsync(User);
            var component = await _commandBus.Send<Component.Create.Command, ComponentAggregate>(
                new Component.Create.Command(creatorId: user.Id)
                );
            return CreatedAtAction(nameof(Get), new { id = component.Id }, component.Id);
        }

        /* [HttpDelete("{id}")] */
        /* public async Task<ActionResult<Component>> DeleteComponent([FromRoute] Guid id) */
        /* { */
        /*     var component = await _dbContext.Components.FindAsync(id); */
        /*     if (component == null) */
        /*     { */
        /*         return NotFound(); */
        /*     } */

        /*     _dbContext.Components.Remove(component); */
        /*     await _dbContext.SaveChangesAsync(); */

        /*     return component; */
        /* } */

        /* private bool ComponentExists(Guid id) */
        /* { */
        /*     return _dbContext.Components.Any(e => e.Id == id); */
        /* } */
    }
}