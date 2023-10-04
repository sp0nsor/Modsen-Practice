using FluentValidation;

namespace CRUDWebAPI.Models.Validators
{
    public class EventDtoValidator : AbstractValidator<EventDto>
    {
        public EventDtoValidator() 
        {
            
            this.RuleFor(x => x.Name)
                .MinimumLength(5)
                .WithMessage("Minimal chars - 5");

            this.RuleFor(x => x.Description)
                .MinimumLength(20)
                .WithMessage("Minimal chars - 20");

            this.RuleFor(x => x.Speaker)
                .Length(3, 10)
                .WithMessage("Сharacter range from 3 to 10");

            this.RuleFor(x => x.Time)
                .Must(date => date > DateTime.Now)
                .WithMessage("Event cannot be in the past");

            this.RuleFor(x => x.Place)
                .MinimumLength(3)
                .WithMessage("Minimal chars - 3");
        }
    }
}
