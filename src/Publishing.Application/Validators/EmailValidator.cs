using FluentValidation;

namespace Publishing.AppLayer.Validators
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                // require domain part to contain a dot
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
