using FluentValidation;

namespace WebServiceLab1.Models
{
    public class RegistrationValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Вкажіть ім'я.")
            .Length(2, 30).WithMessage("Ім'я повинно містити від 2 до 30 символів.");

            RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Вкажіть прізвище.")
            .Length(2, 40).WithMessage("Прізвище повинно містити від 2 до 40 символів.");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Вкажіть електронну пошту.")
            .EmailAddress().WithMessage("Введіть коректну електронну адресу.");

            RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9]{10,15}$")
            .WithMessage("Номер телефону повинен містити від 10 до 15 цифр.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.Age)
            .InclusiveBetween(18, 100)
            .WithMessage("Вік повинен бути від 18 до 100 років.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Вкажіть пароль.")
            .MinimumLength(6).WithMessage("Пароль повинен містити щонайменше 6 символів.");

            RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Підтвердіть пароль.")
            .Equal(x => x.Password).WithMessage("Паролі не співпадають.");

            RuleFor(x => x.City)
            .NotEmpty().WithMessage("Вкажіть місто.")
            .Length(2, 50).WithMessage("Назва міста повинна містити від 2 до 50 символів.");

            RuleFor(x => x.Address)
            .MaximumLength(100)
            .WithMessage("Адреса не повинна перевищувати 100 символів.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

            RuleFor(x => x.Website)
            .Must(website => Uri.TryCreate(website, UriKind.Absolute, out _))
            .WithMessage("Введіть коректну адресу вебсайту.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
        }
    }
}