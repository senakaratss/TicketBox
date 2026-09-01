using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Commands;

namespace TicketBox.Application.Features.Users.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad boş bırakılamaz.");

            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad boş bırakılamaz.");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.");

            RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.");

            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir email giriniz.");

            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor.");
        }
    }
}
