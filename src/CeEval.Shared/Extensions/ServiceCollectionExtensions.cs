using CeEval.Shared.HttpClients;
using CeEval.Shared.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CeEval.Shared.Extensions;
/// <summary>
///   The service collection extensions.
/// </summary>

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   Adds services for the shared project.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <remarks>
    ///   In "appsettings.json" file the following sections should be available:
    ///   <para>"ChannelEngineApi": { "BaseUrl": "", "ApiKey": "" }</para>
    /// </remarks>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddCeEval(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderRepository, OrderRepository2>();

        services.AddTransient<ApiKeyDelegatingHandler>();

        services.AddHttpClient("ChannelEngine", httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.GetSection("ChannelEngineApi:BaseUrl").Value);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<ApiKeyDelegatingHandler>();

        services.AddMediatR(typeof(ServiceCollectionExtensions));

        return services;
    }
}