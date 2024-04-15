using Core.DTOs;
using FluentValidation;


namespace Service.ValidationRules
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            _ = RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adı boş geçilemez.").NotNull().WithMessage("Ürün adı boş geçilemez.");
            _ = RuleFor(x => x.Price).NotEmpty().WithMessage("Ürün fiyatı boş geçilemez.").NotNull().WithMessage("Ürün fiyatı boş geçilemez.");
            _ = RuleFor(x => x.Stock).NotEmpty().WithMessage("Ürün stoğu boş geçilemez.").NotNull().WithMessage("Ürün stoğu boş geçilemez.");
        }
    }
}
