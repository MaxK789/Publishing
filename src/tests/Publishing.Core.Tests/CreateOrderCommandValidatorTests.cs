using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Validators;
using Publishing.AppLayer.Commands;
using FluentValidation;

namespace Publishing.Core.Tests;

[TestClass]
public class CreateOrderCommandValidatorTests
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Validate_InvalidCommand_Throws()
    {
        var validator = new CreateOrderCommandValidator();
        validator.ValidateAndThrow(new CreateOrderCommand("", "", 0, 0, "", ""));
    }

    [TestMethod]
    public void Validate_ValidCommand_Passes()
    {
        var validator = new CreateOrderCommandValidator();
        validator.ValidateAndThrow(new CreateOrderCommand("book", "n", 1, 1, "1", "p"));
    }
}
