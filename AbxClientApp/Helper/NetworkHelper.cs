using System.Net.Sockets;

namespace AbxClientApp.Helper;

public class NetworkHelper
{
    public static TcpClient CreateTcpClient(string host, int port, int timeoutMs = 5000)
     {
         try
         {
             Logger.LogDebug($"Attempting to connect to server {host}:{port}");
             var client = new TcpClient();
             client.Connect(host, port);
             client.ReceiveTimeout = timeoutMs;
             client.SendTimeout = timeoutMs;
             return client;
         }
         catch (Exception ex)
         {
             throw new InvalidOperationException($"Unable to connect to server: {ex.Message}");
         }
     }
}