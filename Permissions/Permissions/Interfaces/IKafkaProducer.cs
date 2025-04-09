namespace Permissions.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceOperationAsync(string operation, int? permissionId);
    }
}
