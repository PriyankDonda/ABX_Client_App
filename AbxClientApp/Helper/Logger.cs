namespace AbxClientApp.Helper;

public static class Logger
{
    public static void LogInfo(string msg) => Log("INFO", ConsoleColor.Cyan, msg);
    public static void LogWarning(string msg) => Log("WARN", ConsoleColor.Yellow, msg);
    public static void LogError(string msg) => Log("ERROR", ConsoleColor.Red, msg);
    public static void LogCritical(string msg) => Log("CRITICAL", ConsoleColor.DarkRed, msg);
    public static void LogDebug(string msg) => Log("DEBUG", ConsoleColor.Gray, msg);

    private static void Log(string level, ConsoleColor color, string msg)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"[{level}] {msg}");
        Console.ForegroundColor = original;
    }
}
