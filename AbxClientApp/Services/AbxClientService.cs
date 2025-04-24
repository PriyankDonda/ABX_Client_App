using System.Net.Sockets;
using AbxClientApp.Helper;
using AbxClientApp.Models;
using static AbxClientApp.Services.PacketService;

namespace AbxClientApp.Services;

public static class AbxClientService
{
    private const string Host = "127.0.0.1";
    private const int Port = 3000;
    private const int PacketSize = 17;
    private const int MaxRetryAttempts = 3;
    
    public static Dictionary<int, Packet> FetchAllPackets()
    {
        var packets = new Dictionary<int, Packet>();

        using var client = NetworkHelper.CreateTcpClient(host: Host, port: Port);
        using var stream = client.GetStream();

        Logger.LogInfo("Sending initial request to fetch all packets.");
        stream.WriteByte(1); // Call type 1

        var buffer = new byte[PacketSize];
        while (true)
        {
            int bytesRead = ReadFull(stream, buffer);
            if (bytesRead == 0) break;

            if (bytesRead < PacketSize)
            {
                Logger.LogWarning($"Incomplete packet received. Expected {PacketSize} bytes but got {bytesRead}.");
                continue;
            }

            var packet = ParsePacket(buffer);
            if (!ValidatePacket(packet))
            {
                Logger.LogWarning($"Invalid packet skipped: {BitConverter.ToString(buffer)}");
                continue;
            }

            Logger.LogDebug($"Received Packet: Seq={packet.Sequence}, Symbol={packet.Symbol}");
            packets[packet.Sequence] = packet;
        }
        
        if(packets.Count > 0)
        Logger.LogInfo($"Initial packet fetch completed. Total packets received: {packets.Count}");

        return packets;
    }

    public static bool RequestMissingPackets(Dictionary<int, Packet> packets)
    {
        if (!packets.Any()) return false;

        int maxSeq = packets.Keys.Max();
        bool allSuccess = true;

        var missingSeqs = Enumerable.Range(1, maxSeq).Where(seq => !packets.ContainsKey(seq)).ToList();
        if (missingSeqs.Any())
        {
            Logger.LogInfo($"Missing packet sequences identified: {string.Join(", ", missingSeqs)}");
        }

        foreach (var seq in missingSeqs)
        {
            bool received = false;
            for (int attempt = 1; attempt <= MaxRetryAttempts && !received; attempt++)
            {
                try
                {
                    using var client = NetworkHelper.CreateTcpClient(host: Host, port: Port);
                    using var stream = client.GetStream();

                    var request = new byte[2];
                    request[0] = 2; // Call type
                    request[1] = (byte)seq;
                    
                    stream.Write(request, 0, request.Length);
                    Logger.LogInfo($"Requesting missing packet seq {seq} (Attempt {attempt})");

                    var buffer = new byte[PacketSize];
                    int bytesRead = ReadFull(stream, buffer);
                    if (bytesRead < PacketSize) throw new IOException("Incomplete packet received");

                    var packet = PacketService.ParsePacket(buffer);
                    if (!PacketService.ValidatePacket(packet)) throw new InvalidDataException("Invalid packet data");

                    packets[packet.Sequence] = packet;
                    Logger.LogInfo($"Successfully received missing packet seq {packet.Sequence}");
                    received = true;
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error fetching seq {seq}, attempt {attempt}: {ex.Message}");
                    if (attempt == MaxRetryAttempts)
                    {
                        Logger.LogError($"Failed to retrieve packet with sequence {seq} after {MaxRetryAttempts} attempts.");
                        allSuccess = false;
                    }

                    Thread.Sleep(100);
                }
            }
            
        }
        
        return allSuccess;
    }

    static int ReadFull(NetworkStream stream, byte[] buffer)
    {
        int totalRead = 0;

        stream.ReadTimeout = 5000;

        try
        {
            int bytesRead = stream.Read(buffer, totalRead, buffer.Length - totalRead);
            totalRead += bytesRead;

            if (bytesRead == 0)
            {
                Logger.LogDebug("Stream closed by server after sending available packets.");
                return 0; 
            }

            return totalRead;
        }
        catch (IOException ex)
        {
            Logger.LogError($"Read attempt failed: {ex.Message}");
            return 0; 
        }
    }
}