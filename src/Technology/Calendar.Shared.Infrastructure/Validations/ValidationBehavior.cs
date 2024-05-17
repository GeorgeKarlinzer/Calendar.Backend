using FluentValidation;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Humanizer;

namespace Calendar.Shared.Infrastructure.Validations;

internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var errors = (await Task.WhenAll(_validators.Select(x => x.ValidateAsync(request, cancellationToken))))
            .SelectMany(x => x.Errors)
            .Where(x => x != null);

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}
