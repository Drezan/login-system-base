using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using login_system.Common;
using login_system.Common.Extensions;
using login_system.DTOs.Requests;
using login_system.Services.Interfaces;

namespace login_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<CreateRoleRequest> _validator;

        public RolesController(IRoleService roleService, IValidator<CreateRoleRequest> validator)
        {
            _roleService = roleService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Validation failed",
                    Errors = validationResult
                    .ToValidationDictionary()
                });
            }

            var role = await _roleService.CreateRoleAsync(request);

            return Ok(role);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}