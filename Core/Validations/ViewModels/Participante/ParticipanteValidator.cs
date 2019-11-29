using System.Threading.Tasks;
using Core.Interfaces.Repositories.Sql;
using Core.ViewModels.Participante;
using Cross.Util.Extensions;
using FluentValidation;

namespace Core.Validations.ViewModels.Participante
{
    public class ParticipanteValidator : AbstractValidator<ParticipanteRequest>
    {
        public ParticipanteValidator(IParticipanteRepository participanteRepo)
        {
            RuleFor(o => o.NomeCompleto)
                .NotEmpty().WithMessage("{PropertyName} é obrigatória");

            RuleFor(o => o.Apelido)
                .NotEmpty().WithMessage("{PropertyName} é obrigatória");

            RuleFor(o => o.Email)
                .NotEmpty().WithMessage("{PropertyName} é obrigatório")
                .MustAsync(async (o, cancellation) => await EmailUnico(o))
                .WithMessage("{PropertyValue} email ja cadastrado")
                .EmailAddress()
                .WithMessage("{PropertyName} inválido");

            async Task<bool> EmailUnico(string email)
            {
                var retorno = await participanteRepo.FindAsync(x => x.Email.Equals(email));
                return retorno.IsNullOrEmpty();
            }

        }
    }
}