using Microsoft.Extensions.Configuration;

namespace Template.Api.Utils.Configuration
{
    public static class EtcdStaticExtensions
    {
        public static IConfigurationBuilder AddEtcdConfiguration(this IConfigurationBuilder builder, EtcdConnectionOptions connectionOptions)
        {
            return builder.Add(new EtcdConfigurationSource(connectionOptions));
        }
    }
}