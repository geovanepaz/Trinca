using Core.ViewModels.Evento;
using FluentValidation;

namespace Core.Validations.ViewModels.Evento
{
    public class EventoValidator : AbstractValidator<EventoRequest>
    {
        public EventoValidator()
        {
            RuleFor(o => o.Descricao)
                .NotEmpty().WithMessage("{PropertyName} é obrigatória");

            RuleFor(o => o.DataEvento)
                .NotEmpty().WithMessage("{PropertyName} é obrigatória");
        }
    }
}