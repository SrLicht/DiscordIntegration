using CommandSystem;
using DiscordIntegration.API;
using NWAPIPermissionSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiscordIntegration.Commands
{
    using static DiscordIntegration;

    internal class ReconnectCommand : ICommand
    {
        public static ReconnectCommand CInstance { get; } = new();

        public string Command { get; } = "reconnect";

        public string[] Aliases { get; } = new[] { "re" };

        public string Description { get; } = "Reconnects the server with the bot";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        { 
            if (!sender.CheckPermission("di.reconnect"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.reconnect");
                return false;
            }

            Instance.DisconnectNetwork();
            NetworkCancellationTokenSource.Cancel();
            NetworkCancellationTokenSource.Dispose();
            Network.Close();

            NetworkCancellationTokenSource = new CancellationTokenSource();
            Network = new Network(Instance.Config.Bot.IPAddress, Instance.Config.Bot.Port, TimeSpan.FromSeconds(Instance.Config.Bot.ReconnectionInterval));
            _ = Network.Start(NetworkCancellationTokenSource);
            Instance.ReconnectNetwork();

            response = "Networking disposed and started a new one";
            return true;
        }
    }
}
