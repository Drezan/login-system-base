using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using login_system.Common;
using login_system.Common.Extensions;
using login_system.DTOs.Requests;
using login_system.Services.Interfaces;

namespace login_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<LoginRequest> _validator;
        public AuthController(IUserService userService, IValidator<LoginRequest> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
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

            var response = await _userService.LoginAsync(request);

            return Ok(response);
        }
    }
}
