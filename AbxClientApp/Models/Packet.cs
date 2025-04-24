namespace AbxClientApp.Models;

public class Packet
{
    public string Symbol { get; set; }
    public string Side { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int Sequence { get; set; }
}
