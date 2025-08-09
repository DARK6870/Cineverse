using Cineverse.Application.MediatR.Support.Commands;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Support;

public class CreateSupportTicketRequestValidator : AbstractValidator<CreateSupportTicketRequest>
{
    public CreateSupportTicketRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name can not be empty");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("First name can not be empty");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");
        
        RuleFor(x => x.Subject)
            .Length(10, 100)
            .WithMessage("Subject must be between 10 and 100 characters");
        
        RuleFor(x => x.Description)
            .Length(30, 300)
            .WithMessage("Subject must be between 30 and 300 characters");
    }
}