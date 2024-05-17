using Calendar.Shared.Abstractions.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.Shared.Infrastructure.Exceptions;

internal class ExceptionCompositionRoot(IServiceProvider serviceProvider) : IExceptionCompositionRoot
{
    public ExceptionResponse Map(Exception exception)
    {
        using var scope = serviceProvider.CreateScope();
        var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToArray();
        var nonDefaultMappers = mappers.Where(x => x is not ExceptionToResponseMapper);
        var result = nonDefaultMappers
            .Select(x => x.Map(exception))
            .SingleOrDefault(x => x is not null);

        if (result is not null)
        {
            return result;
        }

        var defaultMapper = mappers.Single(x => x is ExceptionToResponseMapper);
        return defaultMapper.Map(exception);
    }
}
