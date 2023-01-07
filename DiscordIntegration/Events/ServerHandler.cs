// -----------------------------------------------------------------------
// <copyright file="ServerHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace DiscordIntegration.Events
{
    using API.Commands;
    using Dependency;
    using Respawning;
    using static DiscordIntegration;

    /// <summary>
    /// Handles server-related events.
    /// </summary>
    internal sealed class ServerHandler
    {
#pragma warning disable SA1600 // Elements should be documented

        [PluginEvent(ServerEventType.PlayerCheaterReport)]
        public async void OnReportingCheater(Player issuer, Player target, string reason)
        {
            if (Instance.Config.EventsToLog.ReportingCheater)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Reports, string.Format(Language.CheaterReportFilled, issuer.Nickname, issuer.UserId, issuer.Role, target.Nickname, target.UserId, target.Role, reason))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerReport)]
        public async void OnLocalReporting(Player issuer, Player target, string reason)
        {
            if (Instance.Config.EventsToLog.ReportingCheater)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Reports, string.Format(Language.CheaterReportFilled, issuer.Nickname, issuer.UserId, issuer.Role, target.Nickname, target.UserId, target.Role, reason))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public async void OnWaitingForPlayers()
        {
            if (Instance.Config.EventsToLog.WaitingForPlayers)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, Language.WaitingForPlayers)).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.WaitingForPlayers)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, Language.WaitingForPlayers)).ConfigureAwait(false);
        }
        

        [PluginEvent(ServerEventType.RoundStart)]
        public async void OnRoundStarted()
        {
            if (Instance.Config.EventsToLog.RoundStarted)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.RoundStarting, Player.Count))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public async void OnRoundEnded(RoundSummary.LeadingTeam team)
        {
            if (Instance.Config.EventsToLog.RoundEnded)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.RoundEnded, team, Player.Count, Instance.Slots))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.TeamRespawnSelected)]
        public async void OnRespawningTeam(SpawnableTeamType team)
        {
            if (Instance.Config.EventsToLog.RespawningTeam)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(team == SpawnableTeamType.ChaosInsurgency ? Language.ChaosInsurgencyHaveSpawned : Language.NineTailedFoxHaveSpawned, "ev.Players.Count"))).ConfigureAwait(false);
        }
    }
}