using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Validators;
using Publishing.Core.Commands;
using FluentValidation;

namespace Publishing.Core.Tests;

[TestClass]
public class UpdateProfileCommandValidatorTests
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Validate_Invalid_Throws()
    {
        var validator = new UpdateProfileCommandValidator(new EmailValidator(), new PhoneFaxValidator());
        var cmd = new UpdateProfileCommand { Id = "1", Email = "bad" };
        validator.ValidateAndThrow(cmd);
    }

    [TestMethod]
    public void Validate_Valid_Passes()
    {
        var validator = new UpdateProfileCommandValidator(new EmailValidator(), new PhoneFaxValidator());
        var cmd = new UpdateProfileCommand { Id = "1", FirstName = "A" };
        validator.ValidateAndThrow(cmd);
    }
}
