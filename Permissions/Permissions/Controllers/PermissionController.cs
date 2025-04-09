using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Permissions.Events.Commands;
using Permissions.Events.Queries;
namespace Permissions.Controllers
{


    namespace Permissions.Api.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PermissionsController : ControllerBase
        {
            private readonly IMediator _mediator;
            private readonly ILogger<PermissionsController> _logger;

            public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            [HttpPost("request")]
            public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
            {
                _logger.LogInformation("Starting RequestPermission for employee {FirstName} {LastName}",
                    command.EmployeeFirstName, command.EmployeeLastName);

                try
                {
                    var permissionId = await _mediator.Send(command);
                    _logger.LogInformation("Permission successfully requested with ID: {PermissionId}", permissionId);
                    return Ok(new { PermissionId = permissionId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error requesting permission");
                    return StatusCode(500, "An error occurred while requesting permission");
                }
            }

            [HttpPut("modify/{id}")]
            public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
            {
                _logger.LogInformation("Starting ModifyPermission for ID: {PermissionId}", id);

                try
                {
                    command.Id = id;
                    var result = await _mediator.Send(command);

                    if (result)
                    {
                        _logger.LogInformation("Permission with ID: {PermissionId} modified successfully", id);
                        return Ok();
                    }

                    _logger.LogWarning("Permission with ID: {PermissionId} not found", id);
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error modifying permission with ID: {PermissionId}", id);
                    return StatusCode(500, "An error occurred while modifying permission");
                }
            }

            [HttpGet]
            public async Task<IActionResult> GetPermissions()
            {
                _logger.LogInformation("Starting GetPermissions");

                try
                {
                    var query = new GetPermissionsQuery();
                    var permissions = await _mediator.Send(query);
                    _logger.LogInformation("Retrieved {Count} permissions", permissions.Count());
                    return Ok(permissions);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving permissions");
                    return StatusCode(500, "An error occurred while retrieving permissions");
                }
            }
        }
    }
}
