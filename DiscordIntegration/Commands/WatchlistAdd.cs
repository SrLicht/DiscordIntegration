// -----------------------------------------------------------------------
// <copyright file="PlayerList.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using DiscordIntegration.Dependency.Database;
using NWAPIPermissionSystem;
using PluginAPI.Core;

namespace DiscordIntegration.Commands
{
    using System;
    using CommandSystem;
    using static DiscordIntegration;

    /// <summary>
    /// Adds a user to the watchlist.
    /// </summary>
    internal sealed class WatchlistAdd : ICommand
    {
        public bool SanitizeResponse => true;
        public static WatchlistAdd Instance { get; } = new WatchlistAdd();

        public string Command { get; } = "watchadd";

        public string[] Aliases { get; } = new[] { "wla" };

        public string Description { get; } = "Add player to the watchlist to log when is connected to the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("di.watchlistadd"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.watchlistadd");
                return false;
            }

            Player player = Player.Get(arguments.ElementAt(2));
            string reason = string.Empty;
            foreach (string s in arguments.Skip(2))
                reason += $"{s} ";
            reason = reason.TrimEnd(' ');
            DatabaseHandler.AddEntry(player.UserId, reason);
            response = $"{player.Nickname} added to watchlist for {reason}";
            return true;
        }
    }
}