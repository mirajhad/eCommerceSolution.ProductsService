

namespace BusinessLogicLayer.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(string routingKey, T message);
    }
}
