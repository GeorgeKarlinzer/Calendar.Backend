using Calendar.Shared.Abstractions.Exceptions;

namespace Calendar.Shared.Infrastructure.Exceptions;

internal interface IExceptionCompositionRoot
{
    ExceptionResponse Map(Exception exception);
}
