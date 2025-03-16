using BusinessLogicLayer.DTO;
using FluentValidation;


namespace BusinessLogicLayer.Validators
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator() 
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("Product ID is required");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product Name is required");
            RuleFor(x => x.Category).IsInEnum().WithMessage("Category is required");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Unit Price is required");
            RuleFor(x => x.QuantityInStock).NotEmpty().WithMessage("Quantity In Stock is required");
        }
    }
}
