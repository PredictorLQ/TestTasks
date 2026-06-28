using FluentAssertions;
using FluentValidation.TestHelper;
using OrderTrackingApplication.Application.Commands.Orders.CreateOrder;

namespace OrderTrackingApplication.UnitTests.Validators;

public class CreateOrderCommandValidatorTests
{
    private readonly CreateOrderCommand.CreateOrderCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_OrderNumberIsEmpty()
    {
        var command = new CreateOrderCommand(string.Empty, "Description");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.OrderNumber);
    }

    [Fact]
    public void Should_HaveError_When_DescriptionIsEmpty()
    {
        var command = new CreateOrderCommand("ORD-001", string.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_NotHaveErrors_When_CommandIsValid()
    {
        var command = new CreateOrderCommand("ORD-001", "Valid description");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
