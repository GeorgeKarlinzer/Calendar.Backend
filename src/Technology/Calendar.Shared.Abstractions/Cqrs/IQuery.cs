using MediatR;

namespace Calendar.Shared.Abstractions.Cqrs
{
    public interface IQuery<out T> : IRequest<T>
    {

    }
}
