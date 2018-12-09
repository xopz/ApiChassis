using Microsoft.Extensions.Configuration;

namespace Template.Api.Utils.Configuration
{
    public class EtcdConfigurationSource: IConfigurationSource
    {
        public EtcdConnectionOptions Options { get; set; }

        public EtcdConfigurationSource(EtcdConnectionOptions options)
        {
            this.Options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EtcdConfigurationProvider(this);
        }
    }
}