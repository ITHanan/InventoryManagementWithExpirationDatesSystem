using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.DTOs;

namespace InventoryManagementWithExpirationDatesSystem.Validations
{
    public class StockDTOValidator : AbstractValidator<StockDTO>
    {
        public StockDTOValidator()
        {
            RuleFor(stock => stock.Quantity).GreaterThan(0)
                .WithMessage("Stock quantity must be greater than zero.");
            RuleFor(stock => stock.ItemId).NotEmpty()
                .WithMessage("Item ID is required.");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiry date must be in the future.");
        }

        private bool BeAValidDate(DateOnly expiryDate)
    {
        // Example of date validation, you could add range or format checks if needed
        return expiryDate != null && expiryDate >= DateOnly.FromDateTime(DateTime.Today);
    }
    }

}
