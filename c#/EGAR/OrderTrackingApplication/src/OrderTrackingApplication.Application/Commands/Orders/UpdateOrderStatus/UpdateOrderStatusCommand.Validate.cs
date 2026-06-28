using FluentValidation;
using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Application.Commands.Orders.UpdateOrderStatus;

public sealed partial class UpdateOrderStatusCommand
{
    public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();

            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }
}
