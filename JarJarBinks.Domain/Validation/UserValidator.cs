using FluentValidation;
using JarJarBinks.Domain.Entities;

namespace JarJarBinks.Domain.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(r => r.Name)
                .Cascade(CascadeMode.Continue)
                .NotNull().WithMessage("O Nome não pode ser nulo").WithErrorCode("Null")
                .NotEmpty().WithMessage("O Nome não pode ser vazio").WithErrorCode("Empty")
                .MaximumLength(150).WithMessage("O Nome não pode ser maior que 150 caracteres.").WithErrorCode("GratherThanMaximumLength");
        }
    }
}
