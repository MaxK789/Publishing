using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Validators;
using Publishing.Core.Commands;
using FluentValidation;

namespace Publishing.Core.Tests;

[TestClass]
public class CreateOrganizationCommandValidatorTests
{
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Validate_InvalidCommand_Throws()
    {
        var validator = new CreateOrganizationCommandValidator(new EmailValidator(), new PhoneFaxValidator());
        var cmd = new CreateOrganizationCommand { Name = "", Email = "bad", Phone = "x" };
        validator.ValidateAndThrow(cmd);
    }

    [TestMethod]
    public void Validate_ValidCommand_Passes()
    {
        var validator = new CreateOrganizationCommandValidator(new EmailValidator(), new PhoneFaxValidator());
        var cmd = new CreateOrganizationCommand { Name = "Org", Email = "a@a.com", Phone = "123", Fax = "", Address = "addr", PersonId = "1" };
        validator.ValidateAndThrow(cmd);
    }
}
