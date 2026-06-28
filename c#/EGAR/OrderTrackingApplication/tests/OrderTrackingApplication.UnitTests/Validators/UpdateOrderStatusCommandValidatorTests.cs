using FluentAssertions;
using FluentValidation.TestHelper;
using OrderTrackingApplication.Application.Commands.Orders.UpdateOrderStatus;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.UnitTests.Validators;

public class UpdateOrderStatusCommandValidatorTests
{
    private readonly UpdateOrderStatusCommand.UpdateOrderStatusCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_OrderIdIsEmpty()
    {
        var command = new UpdateOrderStatusCommand(Guid.Empty, OrderStatus.Sent);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.OrderId);
    }

    [Fact]
    public void Should_NotHaveErrors_When_CommandIsValid()
    {
        var command = new UpdateOrderStatusCommand(Guid.NewGuid(), OrderStatus.Delivered);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
