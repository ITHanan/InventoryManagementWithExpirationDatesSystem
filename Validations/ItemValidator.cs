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
                .MaximumLength(20).WithMessage("Item name must not exceed 20 characters.");
        }
    }
}
