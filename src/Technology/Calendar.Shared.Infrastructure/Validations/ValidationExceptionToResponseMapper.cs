using Calendar.Shared.Abstractions.Exceptions;
using FluentValidation;
using System.Net;

namespace Calendar.Shared.Infrastructure.Validations;

internal class ValidationExceptionToResponseMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
    {
        if (exception is not ValidationException validationException)
        {
            return null;
        }

        var errors = validationException.Errors.Select(x => new Error("validation", x.ErrorMessage)).ToArray();

        return new ExceptionResponse(new ErrorsResponse(errors), HttpStatusCode.BadRequest);
    }

    private record Error(string Code, string Message);
    private record ErrorsResponse(params Error[] Errors);
}
