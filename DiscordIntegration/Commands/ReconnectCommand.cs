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

            if (NetworkCancellationTokenSource != null)
            {
                NetworkCancellationTokenSource.Cancel();
                NetworkCancellationTokenSource.Dispose();
            }

            Network.Close();
            Network = null;
            Network = new Network(Instance.Config.Bot.IPAddress, Instance.Config.Bot.Port, TimeSpan.FromSeconds(Instance.Config.Bot.ReconnectionInterval));
            NetworkCancellationTokenSource = new CancellationTokenSource();
            _ = Network.Start(NetworkCancellationTokenSource);

            response = "Se creo un nuevo token y conexion con el servidor";
            return true;
        }
    }
}
