using System.Threading.Tasks;
using Core.Interfaces.Repositories.Sql;
using Core.ViewModels.Usuario;
using Cross.Util.Extensions;
using FluentValidation;

namespace Core.Validations.ViewModels.Usuario
{
    public class UsuarioValidator : AbstractValidator<UsuarioRequest>
    {
        public UsuarioValidator(IUsuarioRepository usuarioRepo)
        {
            RuleFor(o => o.NomeCompleto)
                .NotEmpty().WithMessage("{PropertyName} é obrigatória");

            RuleFor(o => o.Email)
                .NotEmpty().WithMessage("{PropertyName} é obrigatório")
                .MustAsync(async (o, cancellation) => await EmailUnico(o))
                .WithMessage("{PropertyValue} email ja cadastrado")
                .EmailAddress()
                .WithMessage("{PropertyName} inválido");

            RuleFor(o => o.Senha)
                .NotEmpty().WithMessage("{PropertyName} é obrigatória");

            async Task<bool> EmailUnico(string email)
            {
                var retorno = await usuarioRepo.FindAsync(x => x.Email.Equals(email));
                return retorno.IsNullOrEmpty();
            }

        }
    }
}