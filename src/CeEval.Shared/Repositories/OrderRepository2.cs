using CeEval.Shared.Extensions;
using CeEval.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CeEval.Shared.Repositories;

/// <inheritdoc/>
public class OrderRepository2 : IOrderRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderRepository2> _logger;

    /// <summary>
    ///   Initializes a new instance of the <see cref="OrderRepository2"/> class.
    /// </summary>
    /// <param name="httpClientFactor">The HTTP client factory to create <see cref="HttpClient"/>.</param>
    /// <param name="logger">The logger.</param>
    public OrderRepository2(IHttpClientFactory httpClientFactor, ILogger<OrderRepository2> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactor.CreateClient("ChannelEngine");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Order>> GetOrdersWithLinesByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        var url = $"api/v2/orders?statuses={Utility.GetEnumMemberValue(status)}";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Cannot get list of orders form ChannelEngine API. Stats code: {Status} - Content: " + content, response.StatusCode);
            return new List<Order>();
        }

        var items = Utility.DeserializeAnonymousType(content, new
        {
            Content = new[]
            {
                new
                    {
                        Id = 0,
                        ChannelId = (int?)null,
                        ChannelName = "",
                        Status = "",
                        OrderDate = (DateTimeOffset?) null,
                        Lines = new[] { new { Gtin = "", Description = "", ChannelProductNo = "", MerchantProductNo = "", Quantity = 0 } }
                    }
            }
        }).Content;

        var orders = items.Select(o => new Order
        {
            Id = o.Id,
            ChannelId = o.ChannelId,
            ChannelName = o.ChannelName,
            OrderDate = o.OrderDate,
            Status = o.Status.ToEnum<OrderStatus>(),
            Lines = o.Lines.Select(ol => new OrderLine
            {
                ChannelProductNo = ol.ChannelProductNo,
                Description = ol.Description,
                Gtin = ol.Gtin,
                MerchantProductNo = ol.MerchantProductNo,
                Quantity = ol.Quantity
            })
        }).ToList();

        return orders;
    }
}