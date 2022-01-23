using CeEval.Shared.Models;
using MediatR;

namespace CeEval.Shared.Queries
{
    /// <summary>
    ///   Get list of top sold products.
    /// </summary>
    /// <param name="OrderStatus">Filter orders by status.</param>
    /// <param name="Count">Total products to return.</param>
    public record ProductTopSoldListQuery(OrderStatus OrderStatus, int Count)
        : IRequest<IEnumerable<ProductTopSoldDto>>;
}