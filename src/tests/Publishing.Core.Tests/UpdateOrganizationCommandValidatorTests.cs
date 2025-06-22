using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Validators;
using Publishing.Core.Commands;
using FluentValidation;

namespace Publishing.Core.Tests;

[TestClass]
public class UpdateOrganizationCommandValidatorTests
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Validate_Invalid_Throws()
    {
        var validator = new UpdateOrganizationCommandValidator(new EmailValidator(), new PhoneFaxValidator());
        var cmd = new UpdateOrganizationCommand { Id = "1", Email = "bad", Phone = "x" };
        validator.ValidateAndThrow(cmd);
    }

    [TestMethod]
    public void Validate_Valid_Passes()
    {
        var validator = new UpdateOrganizationCommandValidator(new EmailValidator(), new PhoneFaxValidator());
        var cmd = new UpdateOrganizationCommand { Id = "1", Name = "Org", Email = "a@a.com" };
        validator.ValidateAndThrow(cmd);
    }
}
