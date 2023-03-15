// -----------------------------------------------------------------------
// <copyright file="MapHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using MapGeneration;
using MapGeneration.Distributors;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Scp914;
using UnityEngine;

namespace DiscordIntegration.Events
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Dependency;
    using static DiscordIntegration;

    /// <summary>
    /// Handles map-related events.
    /// </summary>
    internal sealed class MapHandler
    {
        private readonly DiscordIntegration Plugin;
        public MapHandler(DiscordIntegration plugin)
        {
            Plugin = plugin;
        }
        
#pragma warning disable SA1600 // Elements should be documented
        [PluginEvent(ServerEventType.WarheadDetonation)]
        public async void OnWarheadDetonated()
        {
            if (Instance.Config.EventsToLog.WarheadDetonated)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, Language.WarheadHasDetonated)).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.GeneratorActivated)]
        public async void OnGeneratorActivated(Scp079Generator generator)
        {
            if (Instance.Config.EventsToLog.GeneratorActivated)
            {
                var generatorRoom = RoomIdUtils.RoomAtPosition(generator.gameObject.transform.position);
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorFinished, generatorRoom.Name, Events.VeryHelpful.GeneratorCount++))).ConfigureAwait(false);
            }
        }
        
        [PluginEvent(ServerEventType.LczDecontaminationStart)]
        public async void OnDecontaminating()
        {
            if (Instance.Config.EventsToLog.Decontaminating)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, Language.DecontaminationHasBegun)).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.WarheadStart)]
        public async void OnStartingWarhead(bool isAutomatic, Player ply, bool isResumed)
        {
            if (Instance.Config.EventsToLog.StartingWarhead && (ply == null || (ply != null && (ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))))
            {
                object[] vars = ply == null ?
                    new object[] { Warhead.DetonationTime } :
                    new object[] { ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, Warhead.DetonationTime };

                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(ply == null ? Language.WarheadStarted : Language.PlayerWarheadStarted, vars))).ConfigureAwait(false);
            }
        }

        [PluginEvent(ServerEventType.WarheadStop)]
        public async void OnStoppingWarhead(Player ply)
        {
            if (Instance.Config.EventsToLog.StoppingWarhead && (ply == null || (ply != null && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))))
            {
                object[] vars = ply == null ?
                    Array.Empty<object>() :
                    new object[] { ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role };

                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(ply == null ? Language.CanceledWarhead : Language.PlayerCanceledWarhead, vars))).ConfigureAwait(false);
            }

            if (Instance.Config.StaffOnlyEventsToLog.StoppingWarhead)
            {
                object[] vars = ply == null
                    ? Array.Empty<object>()
                    : new object[] { ply.Nickname, ply.UserId, ply.Role };

                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(ply == null ? Language.CanceledWarhead : Language.PlayerCanceledWarhead, vars))).ConfigureAwait(false);
            }
        }

        [PluginEvent(ServerEventType.Scp914UpgradeInventory)]
        public async void OnUpgradingItemsInventory(Player ply, ItemBase item, Scp914KnobSetting knobSetting)
        {
            if (Instance.Config.EventsToLog.UpgradingScp914Items)
            {
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914ProcessedItem, item.ItemTypeId)));
            }
        }
        
        [PluginEvent(ServerEventType.Scp914PickupUpgraded)]
        public async void OnUpgradingItemsPickup(ItemPickupBase item, Vector3 outPosition, Scp914KnobSetting knobSetting)
        {
            if (Instance.Config.EventsToLog.UpgradingScp914Items)
            {
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914ProcessedItem, item.NetworkInfo.ItemId)));
            }
        }
    }
}