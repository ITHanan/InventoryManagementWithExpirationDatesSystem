using FluentValidation;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Models;

namespace InventoryManagementWithExpirationDatesSystem.Validations
{
    public class ItemDTOValidator : AbstractValidator<ItemDTO>
    {
        public ItemDTOValidator()
        {
            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Item name is required.")
                .MaximumLength(50).WithMessage("Item name must not exceed 50 characters.");

            RuleFor(x => x.UnitPrice)
                .NotNull().WithMessage("Unit price is required.")
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");

            //RuleFor(x => x.StockQuantity)
            //    .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

            //RuleFor(x => x.ExpiryDate)
            //    .GreaterThan(DateTime.UtcNow).WithMessage("Expiry date must be a future date.")
            //    .When(x => x.ExpiryDate != null);

            RuleFor(x => x.Category)
                .MaximumLength(30).WithMessage("Category must not exceed 30 characters.")
                .When(x => !string.IsNullOrEmpty(x.Category));
        }
    }
}
