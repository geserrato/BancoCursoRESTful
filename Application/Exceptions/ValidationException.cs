using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    private ValidationException() : base("Se han producido uno o más errores de validación")
    {
        Errors = new List<string>();
    }

    public List<string>? Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures): this()
    {
        foreach (ValidationFailure failure in failures)
        {
            Errors?.Add(failure.ErrorMessage);
        }
    }
}