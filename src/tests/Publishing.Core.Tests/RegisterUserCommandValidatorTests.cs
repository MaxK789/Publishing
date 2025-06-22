using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Validators;
using Publishing.Core.Commands;

namespace Publishing.Core.Tests;

[TestClass]
public class RegisterUserCommandValidatorTests
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Validate_InvalidCommand_Throws()
    {
        var validator = new RegisterUserCommandValidator(new EmailValidator());
        validator.ValidateAndThrow(new RegisterUserCommand());
    }

    [TestMethod]
    public void Validate_ValidCommand_Passes()
    {
        var validator = new RegisterUserCommandValidator(new EmailValidator());
        var cmd = new RegisterUserCommand
        {
            FirstName = "a",
            LastName = "b",
            Email = "test@example.com",
            Status = "contact",
            Password = "pass"
        };
        validator.ValidateAndThrow(cmd);
    }
}
