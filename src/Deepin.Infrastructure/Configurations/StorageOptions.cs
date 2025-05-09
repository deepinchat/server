using Deepin.Domain.FileAggregate;
using Newtonsoft.Json.Linq;

namespace Deepin.Infrastructure.Configurations;

public class StorageOptions
{
    public StorageProvider Provider { get; set; } = StorageProvider.Local;
    public required JObject Config { get; set; }
}
