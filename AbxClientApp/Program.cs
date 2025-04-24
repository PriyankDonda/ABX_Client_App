using AbxClientApp.Helper;
using AbxClientApp.Services;

namespace AbxClientApp;

class Program
{
    static void Main()
    {
        try
        {
            Logger.LogInfo("Starting ABX Client...");
            var packets = AbxClientService.FetchAllPackets();
            
            if (packets.Count == 0)
            {
                Logger.LogCritical("No packets received from server. Exiting.");
                return;
            }

            bool allReceived = AbxClientService.RequestMissingPackets(packets);
            
            if (!allReceived)
            {
                Logger.LogCritical("Some packets could not be retrieved after maximum attempts. JSON will not be generated.");
                return;
            }
            
            FileService.WriteToJson(packets.Values.OrderBy(p => p.Sequence));
            Logger.LogInfo("ABX Client completed successfully.");
        }
        catch (Exception ex)
        {
            Logger.LogCritical($"Fatal error in ABX Client: {ex.Message}");
        }
    }
}