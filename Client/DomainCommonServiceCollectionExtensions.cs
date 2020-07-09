using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Client
{
    public static class DomainCommonServiceCollectionExtensions
    {
        public static IServiceCollection AddRestResourceClient(this IServiceCollection services)
        {
            // Performance note: The http client needs to be singleton to prevent the website from running out of connections.
            services.TryAddSingleton(provider =>
            {
                var factory = provider.GetService<IHttpClientFactory>();
                return factory.CreateHttpClient();
            });

            services.TryAddSingleton<IRESTResourceClient, RESTResourceClient>();
            return services;
        }
    }
}
