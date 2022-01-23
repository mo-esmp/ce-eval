namespace CeEval.Shared.Models;

public class Order
{
    public Order()
    {
        Lines = new List<OrderLine>();
    }

    public int Id { get; set; }

    public int? ChannelId { get; set; }

    public string ChannelName { get; set; }

    public OrderStatus Status { get; set; }

    public DateTimeOffset? OrderDate { get; set; }

    public IEnumerable<OrderLine> Lines { get; set; }
}