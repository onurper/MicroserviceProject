using Core.DTOs;
using FluentValidation;


namespace Service.ValidationRules
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            _ = RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta alanı boş geçilemez.").EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");
            _ = RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş geçilemez.");
        }
    }
}
