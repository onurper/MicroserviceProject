using Core.DTOs;
using FluentValidation;

namespace Service.ValidationRules
{
    public class RegisterValidator : AbstractValidator<CreateUserDto>
    {
        public RegisterValidator()
        {
            _ = RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı alanı boş geçilemez.").NotNull().WithMessage("Kullanıcı adı alanı boş geçilemez.");
            _ = RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta alanı boş geçilemez.").EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");
            _ = RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş geçilemez.");
        }
    }
}