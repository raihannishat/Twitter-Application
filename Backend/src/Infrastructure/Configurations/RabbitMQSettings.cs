namespace Infrastructure.Configurations;

public class RabbitMQSettings : IRabbitMQSettings
{
    public string URI { get; set; } = string.Empty;
}
