using FluentValidation;
using MediatR;

namespace Calendar.Shared.Infrastructure.Validations;

internal class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var errors = (await Task.WhenAll(validators.Select(x => x.ValidateAsync(request, cancellationToken))))
            .SelectMany(x => x.Errors)
            .Where(x => x != null);

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}
