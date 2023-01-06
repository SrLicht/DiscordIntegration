// -----------------------------------------------------------------------
// <copyright file="StaffList.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using NWAPIPermissionSystem;
using PluginAPI.Core;

namespace DiscordIntegration.Commands
{
    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;
    using static DiscordIntegration;

    /// <summary>
    /// Gets the list of staffers in the server.
    /// </summary>
    internal sealed class StaffList : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private StaffList()
        {
        }

        public static StaffList Instance { get; } = new StaffList();

        public string Command { get; } = "stafflist";

        public string[] Aliases { get; } = new[] { "sl" };

        public string Description { get; } = Language.StaffListCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission("di.stafflist"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.stafflist");
                return false;
            }

            StringBuilder message = StringBuilderPool.Shared.Rent();

            foreach (Player player in Player.GetPlayers())
            {
                if (player.RemoteAdminAccess)
                {
                    var groupname =
                        ServerStatic.PermissionsHandler._members.TryGetValue(player.UserId, out string groupName);
                    message.Append(player.Nickname).Append(" - ").Append(groupname ? groupName : "").AppendLine();
                }
                    
            }

            if (message.Length == 0)
                message.Append(Language.NoStaffOnline);

            response = message.ToString();

            StringBuilderPool.Shared.Return(message);

            return true;
        }
    }
}
