using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using login_system.Common;
using login_system.Common.Extensions;
using login_system.DTOs.Requests;
using login_system.Services.Interfaces;

namespace login_system.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserRequest> _validator;
        private readonly IValidator<UpdateUserRequest> _updateValidator;
        private readonly IValidator<UpdatePasswordRequest> _passwordValidator;

        public UsersController(IUserService userService, IValidator<RegisterUserRequest> validator, IValidator<UpdateUserRequest> updateValidator, IValidator<UpdatePasswordRequest> passwordValidator  )
        {
            _userService = userService;
            _validator = validator;
            _updateValidator = updateValidator;
            _passwordValidator = passwordValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Validation failed",
                    Errors = validationResult.ToValidationDictionary()
                });
            }

            var user = await _userService.CreateUserAsync(request);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Validation failed",
                    Errors = validationResult.ToValidationDictionary()
                });
            }

            var user = await _userService.UpdateUserAsync(id, request);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            return Ok(user);
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(Guid id, UpdatePasswordRequest request)
        {
            var currentUserId = User.GetUserId();
            var isAdmin = User.IsAdmin();

            if (currentUserId != id && !isAdmin)
            {
                return Forbid();
            }

            var validationResult = await _passwordValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Validation failed",
                    Errors = validationResult.ToValidationDictionary()
                });
            }

            await _userService.UpdatePasswordAsync(id, request);
            return NoContent();
        }
    }
}