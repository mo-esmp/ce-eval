namespace CeEval.Shared.Models;

public class OrderLine
{
    public string Gtin { get; set; }

    public string Description { get; set; }

    public string ChannelProductNo { get; set; }

    public string MerchantProductNo { get; set; }

    public int Quantity { get; set; }
}