using FluentValidation;
using login_system.DTOs.Requests;

namespace login_system.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }
}