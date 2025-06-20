using FluentValidation;

namespace Publishing.AppLayer.Validators
{
    /// <summary>
    /// Validates phone and fax strings allowing only digits when provided.
    /// </summary>
    public class PhoneFaxValidator : AbstractValidator<string>
    {
        public PhoneFaxValidator()
        {
            RuleFor(x => x)
                .Must(v => string.IsNullOrEmpty(v) || v.All(char.IsDigit))
                .WithMessage("Invalid phone/fax format");
        }
    }
}
