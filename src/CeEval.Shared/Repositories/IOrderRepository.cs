using CeEval.Shared.Models;

namespace CeEval.Shared.Repositories;

/// <summary>
///   The order repository.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    ///   Gets the orders with lines by status asynchronous.
    /// </summary>
    /// <param name="status">The order status.</param>
    /// <param name="cancellationToken">The cancellation token (optional.</param>
    /// <returns>A Task contains list of orders.</returns>
    public Task<IEnumerable<Order>> GetOrdersWithLinesByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
}