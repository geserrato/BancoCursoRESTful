using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Behaviours;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any()) return await next();
        
        var context = new ValidationContext<TRequest>(request);
        ValidationResult[] validationsResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
        IEnumerable<ValidationFailure> failures = validationsResults.SelectMany(validationResult => validationResult.Errors).Where(validationFailure => validationFailure != null).ToList();

        if (failures.Any())
        {
            throw new Exceptions.ValidationException(failures);
        }

        return await next();
    }
}