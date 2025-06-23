using FluentValidation;
using System.Text.RegularExpressions;

namespace Publishing.AppLayer.Validators
{
    /// <summary>
    /// Validates phone and fax strings using the E.164 international format.
    /// </summary>
    public class PhoneFaxValidator : AbstractValidator<string>
    {
        public PhoneFaxValidator()
        {
            RuleFor(x => x)
                .Must(v => string.IsNullOrEmpty(v) || Regex.IsMatch(v, "^\\+?[1-9]\\d{2,14}$"))
                .WithMessage("Invalid phone/fax format");
        }
    }
}
