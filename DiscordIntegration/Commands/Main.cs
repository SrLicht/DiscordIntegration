// -----------------------------------------------------------------------
// <copyright file="Main.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands
{
#pragma warning disable SA1600 // Elements should be documented
    using System;
    using CommandSystem;
    using GameCore;
    using MEC;
    using PluginAPI.Core;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class Main : ParentCommand
    {
        public bool SanitizeResponse => true;

        public override string Command { get; } = "discordintegration";

        public override string[] Aliases { get; } = new[] { "di" };

        public override string Description { get; } = "Main command for DiscordIntegration";
        
        public Main() => LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
            //RegisterCommand(PlayerList.Instance);
            //RegisterCommand(StaffList.Instance);
            try
            {
                RegisterCommand(ReconnectCommand.CInstance);

                RegisterCommand(WatchlistAdd.Instance);
                RegisterCommand(WatchlistRemove.Instance);
                LateDisable();
            }
            catch (Exception e )
            {
                PluginAPI.Core.Log.Error($"Error registering commands: {e} | {e.GetType()} {DiscordIntegration.Instance is null}");
            }
        }


        private void LateDisable()
        {
            Timing.CallDelayed(3, () =>
            {
                if (!DiscordIntegration.Instance.Config.UseWatchlist)
                {
                    UnregisterCommand(WatchlistAdd.Instance);
                    UnregisterCommand(WatchlistRemove.Instance);
                }
            });
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"{DiscordIntegration.Language.InvalidSubcommand} {DiscordIntegration.Language.Available}: reconnect " + $"{(DiscordIntegration.Instance.Config.UseWatchlist ? "watchadd, watchrem" : string.Empty)}";
            return false;
        }
    }
}
