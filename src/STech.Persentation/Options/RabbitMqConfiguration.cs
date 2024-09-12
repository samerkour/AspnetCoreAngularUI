namespace STech.Presentation.Api.Options
{
    public class RabbitMqConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public ushort Port { get; set; } = 5670;
        public string VirtualHost { get; set; } = "/"; 
        public string APIUrl { get; set; }
        public string APIKey { get; set; }
        public string APISecretKey { get; set; }
        public ushort PrefetchMessagesCount { get; set; }
        public int DeliveryDelay { get; set; }

    }
}
