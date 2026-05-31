using FluentValidation;
using login_system.DTOs.Requests;

namespace login_system.Validators
{
    public class UpdatePasswordRequestValidator :AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(12).WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(128).WithMessage("New password must be no more than 128 characters long.");
        }
    }
}