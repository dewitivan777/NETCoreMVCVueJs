using Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiClientServiceCollectionExtentions
    {
        public static IServiceCollection AddApiClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ClientOptions>(configuration);
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();

            services.AddSingleton<IApiClient, ApiClient>();
            
            //Performance note: The http client needs to be singleton to prevent the website from running out of connections.
            services.TryAddSingleton(provider =>
            {
                var factory = provider.GetService<IHttpClientFactory>();
                return factory.CreateHttpClient();
            });            
            return services;
        }
    }
}