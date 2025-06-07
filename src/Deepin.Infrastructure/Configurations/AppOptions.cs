using Deepin.Infrastructure.Caching;

namespace Deepin.Infrastructure.Configurations;

public class AppOptions
{
    public required ConnectionStringOptions ConnectionStrings { get; set; }
    public RedisCacheOptions? Redis { get; set; }
    public RabbitMqOptions? RabbitMq { get; set; }
    public SmtpOptions? Smtp { get; set; }
    public StorageOptions? Storage { get; set; }
}
