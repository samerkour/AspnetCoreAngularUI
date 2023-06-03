using MassTransit;

namespace STech.Services.Employee.Api.Helpers
{
    public class RabbitMqEntityNameFormatter : IEntityNameFormatter
    {
        string Source = "Employee.ApiService";
        string Destination = "BlockchainService";
        public string FormatEntityName<T>()
        {
            return $"{Source}.{Destination}.{typeof(T).Name}";
        }
    }
}
