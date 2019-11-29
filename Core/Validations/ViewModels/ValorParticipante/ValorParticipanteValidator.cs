using System.Threading.Tasks;
using Core.Interfaces.Repositories.Sql;
using Core.ViewModels.Evento.ValorParticipante;
using Cross.Util.Extensions;
using FluentValidation;

namespace Core.Validations.ViewModels.ValorParticipante
{
    public class ValorParticipanteValidator : AbstractValidator<ValorParticipanteRequest>
    {
        public ValorParticipanteValidator(IValorParticipanteRepository valorParticipanterepo, IParticipanteRepository participanteRepo, IEventoRepository eventoRepo)
        {
            RuleFor(o => o.IdEvento)
                .NotEmpty()
                .WithMessage("{PropertyName} é obrigatório")
                .MustAsync(async (o, cancellation) => await ExistirEvento(o))
                .WithMessage("{PropertyName} nao encontrado");

            RuleFor(o => o.IdParticipante)
                .NotEmpty()
                .WithMessage("{PropertyName} é obrigatório")
                .MustAsync(async (o, cancellation) => await ExistirParticipante(o))
                .WithMessage("{PropertyName} nao encontrado");


            RuleFor(o => new { o.IdEvento, o.IdParticipante })
                .MustAsync(async (o, cancellation) => await ConterRegistroUnico(o.IdEvento, o.IdParticipante))
                .WithMessage("Participante ja cadastrado para esse evento.")
                .OverridePropertyName("Evento e participante");

            async Task<bool> ConterRegistroUnico(int idEvento, int idParticipante)
            {
                if (idEvento.IsAnyNullOrEmpty() == true || idParticipante.IsAnyNullOrEmpty() == true)
                {
                    return true;
                }
                var retorno = await valorParticipanterepo.FindAsync(x => x.IdParticipante == idParticipante && x.IdEvento == idEvento);
                return retorno.IsNullOrEmpty();
            }

            async Task<bool> ExistirParticipante(int idParticipante)
            {
                if (idParticipante.IsAnyNullOrEmpty() == true || idParticipante == 0)
                {
                    return true;
                }

                var retorno = await participanteRepo.FindAsync(x => x.Id == idParticipante);
                return !retorno.IsNullOrEmpty();
            }

            async Task<bool> ExistirEvento(int idEvento)
            {
                if (idEvento.IsAnyNullOrEmpty() == true || idEvento == 0)
                {
                    return true;
                }

                var retorno = await eventoRepo.FindAsync(x => x.Id == idEvento);
                return !retorno.IsNullOrEmpty();
            }
        }
    }
}