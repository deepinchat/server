using Deepin.API.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Deepin.API.Extensions;
public static class ControllersExtensions
{
    public static IServiceCollection AddDefaultControllers(this IServiceCollection services)
    {
        services
        .AddControllers(options =>
        {
            options.Filters.Add<HttpGlobalExceptionFilter>();
        })
        .AddNewtonsoftJson(options =>
        {
            var jsonSettings = options.SerializerSettings;
            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonSettings.Converters = [
                new StringEnumConverter(){ NamingStrategy = new CamelCaseNamingStrategy()}
                ];
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            JsonConvert.DefaultSettings = () => jsonSettings;
        });
        return services;
    }
}
