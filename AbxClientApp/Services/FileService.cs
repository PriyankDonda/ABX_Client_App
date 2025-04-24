using System.Text.Json;
using AbxClientApp.Helper;
using AbxClientApp.Models;

namespace AbxClientApp.Services;

public static class FileService
{
    public static void WriteToJson(IEnumerable<Packet> packets)
    {
        Logger.LogInfo("Preparing to write packets to JSON file...");

        string baseDir = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.FullName ?? AppContext.BaseDirectory;
        string outputDir = Path.Combine(baseDir, "output");
        Directory.CreateDirectory(outputDir);

        string outputPath = Path.Combine(outputDir, "output.json");
        var json = JsonSerializer.Serialize(packets, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);

        Logger.LogInfo($"Total packets written to JSON: {packets.Count()}");
        Logger.LogInfo($"Output written to: {outputPath}");
    }
}
