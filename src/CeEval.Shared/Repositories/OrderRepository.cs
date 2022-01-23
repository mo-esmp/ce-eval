using CeEval.Shared.Models;

namespace CeEval.Shared.Repositories;

/// <inheritdoc/>
public class OrderRepository : IOrderRepository
{
    private readonly HttpClient _httpClient;

    /// <summary>
    ///   Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="httpClientFactor">The HTTP client factory to create <see cref="HttpClient"/>.</param>
    public OrderRepository(IHttpClientFactory httpClientFactor)
    {
        _httpClient = httpClientFactor.CreateClient("ChannelEngine");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Order>> GetOrdersWithLinesByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        var client = new ChannelEngineRestClient(_httpClient)
        {
            BaseUrl = _httpClient.BaseAddress?.ToString()
        };
        var orderStatusView = (OrderStatusView)status;

        var response = await client.OrderGetByFilterAsync(statuses: new[] { orderStatusView }, null, null, null, null, null,
            null, null, null, null, null, null, null, null, cancellationToken);

        if (response?.Content == null)
            return new List<Order>();

        return response.Content.Select(o => new Order
        {
            ChannelId = o.ChannelId,
            ChannelName = o.ChannelName,
            Id = o.Id,
            OrderDate = o.OrderDate,
            Status = (OrderStatus)o.Status,
            Lines = o.Lines == null
                ? new List<OrderLine>()
                : o.Lines.Select(ol => new OrderLine
                {
                    ChannelProductNo = ol.ChannelProductNo,
                    Description = ol.Description,
                    Gtin = ol.Gtin,
                    MerchantProductNo = ol.MerchantProductNo,
                    Quantity = ol.Quantity
                })
        });
    }
}