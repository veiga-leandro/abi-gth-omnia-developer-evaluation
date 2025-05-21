namespace Ambev.DeveloperEvaluation.Common.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : class;
    }
}
