using CeEval.Shared.Models;
using CeEval.Shared.Repositories;
using MediatR;

namespace CeEval.Shared.Queries;

/// <summary>
///   Handles order queries.
/// </summary>
public class OrderQueryHandler : IRequestHandler<ProductTopSoldListQuery, IEnumerable<ProductTopSoldDto>>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    ///   Initializes a new instance of the <see cref="OrderQueryHandler"/> class.
    /// </summary>
    /// <param name="orderRepository">The order repository.</param>
    public OrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    ///   Handles the OrderListQuery
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task.</returns>
    public async Task<IEnumerable<ProductTopSoldDto>> Handle(ProductTopSoldListQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersWithLinesByStatusAsync(request.OrderStatus, cancellationToken);

        var topSoldProducts = orders.Where(order => order.Lines != null)
            .SelectMany(o => o.Lines)
            .GroupBy(g => g.MerchantProductNo)
            .Select(g => new { Gtin = g.Key, Quantity = g.Sum(ol => ol.Quantity), ProductName = g.First().Description })
            .OrderByDescending(g => g.Quantity)
            .Take(request.Count)
            .Select(g => new ProductTopSoldDto(g.Gtin, g.ProductName, g.Quantity))
            .ToList();

        return topSoldProducts;
    }
}