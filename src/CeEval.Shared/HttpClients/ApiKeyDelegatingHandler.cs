using Microsoft.Extensions.Configuration;

namespace CeEval.Shared.HttpClients;

public class ApiKeyDelegatingHandler : DelegatingHandler
{
    private readonly string _apiKey;

    public ApiKeyDelegatingHandler(IConfiguration configuration)
    {
        _apiKey = configuration.GetSection("ChannelEngineApi:ApiKey").Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.RequestUri = new Uri($"{request.RequestUri}&apikey={_apiKey}");

        return base.SendAsync(request, cancellationToken);
    }
}