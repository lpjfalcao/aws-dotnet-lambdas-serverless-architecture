using Curriculo4Dev.Core.Domain.Entities;
using FluentValidation;

namespace Curriculo4Dev.Core.Domain.Validators.Usuarios
{
    public class AssinarPlanoUsuarioValidator : AbstractValidator<Usuario>
    {
        public AssinarPlanoUsuarioValidator()
        {
            RuleFor(prop => prop.Atributos.Nome)
            .NotNull()
            .WithMessage("O nome do usuário é de preenchimento obrigatório");

            RuleFor(prop => prop.Atributos.Email)
                .NotNull().WithMessage("O e-mail do usuário é de preenchimento obrigatório")
                .EmailAddress().WithMessage("O e-mail informado é inválido");

            RuleFor(prop => prop.Atributos.Username)
                .NotNull()
                .WithMessage("O username é de preenchimento obrigatório");

            RuleFor(prop => prop.Atributos.Plano)
                .NotNull()
                .WithMessage("O plano do usuário é de preenchimento obrigatório");
        }
    }
}
