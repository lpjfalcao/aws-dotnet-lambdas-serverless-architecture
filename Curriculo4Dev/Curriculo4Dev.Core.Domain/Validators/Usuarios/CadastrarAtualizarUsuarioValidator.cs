using Curriculo4Dev.Core.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curriculo4Dev.Core.Domain.Validators.Usuarios
{
    public class CadastrarAtualizarUsuarioValidator : AbstractValidator<Usuario>
    {
        public CadastrarAtualizarUsuarioValidator()
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
        }
    }
}
