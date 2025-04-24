using System.Text;
using AbxClientApp.Models;

namespace AbxClientApp.Services;

public static class PacketService
{
    public static Packet ParsePacket(byte[] data)
    {
        return new Packet
        {
            Symbol = Encoding.ASCII.GetString(data, 0, 4),
            Side = Encoding.ASCII.GetString(data, 4, 1),
            Quantity = BitConverter.ToInt32(data.AsSpan(5, 4).ToArray().Reverse().ToArray()),
            Price = BitConverter.ToInt32(data.AsSpan(9, 4).ToArray().Reverse().ToArray()),
            Sequence = BitConverter.ToInt32(data.AsSpan(13, 4).ToArray().Reverse().ToArray())
        };
    }

    public static bool ValidatePacket(Packet p)
    {
        return !string.IsNullOrWhiteSpace(p.Symbol)
               && (p.Side == "B" || p.Side == "S")
               && p.Quantity > 0
               && p.Price > 0
               && p.Sequence > 0;
    }
}
