using FluentValidation;

namespace Application.Features.Clients.Commands.CreateClientCommand;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
            .MaximumLength(80).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres");
        
        RuleFor(p => p.Surname)
            .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
            .MaximumLength(80).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres");

        RuleFor(p => p.BirthDate)
            .NotEmpty().WithMessage("Fecha de nacimiento no puede ser vacia.");

        RuleFor(p => p.Phone)
            .NotEmpty().WithMessage("El telefono no puede ser vacia.")
            .Matches(@"^\d{4}-\d{4}$").WithMessage("El telefono debe cumplir el formato 0000-0000")
            .MaximumLength(9).WithMessage("El telefono no debe exceder {MaxLength} caractres");
        
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
            .EmailAddress().WithMessage("{PropertyName} debe ser una dirección de email valida")
            .MaximumLength(100).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres");
        
        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
            .MaximumLength(120).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres");
    }
}