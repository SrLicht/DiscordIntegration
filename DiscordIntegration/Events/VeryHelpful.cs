using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace DiscordIntegration.Events;

public class VeryHelpful
{
    private readonly DiscordIntegration Plugin;
    public VeryHelpful(DiscordIntegration plugin)
    {
        Plugin = plugin;
    }
    
    public static ushort GeneratorCount = 0;
    
    [PluginEvent(ServerEventType.MapGenerated)]
    void OnGenerateMap()
    {
        GeneratorCount = 0;
    }

    [PluginEvent]
    public void OnCommandExecuted(ConsoleCommandExecutedEvent ev)
    {
        if(ev.Command.ToLower() == "sr" || ev.Command.ToLower() == "restart")
        {
            DiscordIntegration.NetworkCancellationTokenSource.Cancel();
            DiscordIntegration.NetworkCancellationTokenSource.Dispose();
        }
    }
}