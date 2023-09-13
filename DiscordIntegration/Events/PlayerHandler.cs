// -----------------------------------------------------------------------
// <copyright file="PlayerHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using DiscordIntegration.Dependency.Database;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using MapGeneration.Distributors;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Scp914;
using UnityEngine;

namespace DiscordIntegration.Events
{
    using System;
    using Dependency;
    using PluginAPI.Events;
    using static DiscordIntegration;

    /// <summary>
    /// Handles player-related events.
    /// </summary>
    internal sealed class PlayerHandler
    {
        private readonly DiscordIntegration Plugin;
        public PlayerHandler(DiscordIntegration plugin)
        {
            Plugin = plugin;
        }
        
#pragma warning disable SA1600 // Elements should be documented
        [PluginEvent(ServerEventType.PlayerActivateGenerator)]
        public async void OnInsertingGeneratorTablet(Player ply, Scp079Generator generator)
        {
            if (Instance.Config.EventsToLog.PlayerActivatedGenerator && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorInserted, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerActivatedGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorInserted, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }
        

        [PluginEvent(ServerEventType.PlayerOpenGenerator)]
        public async void OnOpeningGenerator(Player ply, Scp079Generator generator)
        {
            if (Instance.Config.EventsToLog.PlayerOpeningGenerator && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorOpened, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerOpeningGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorOpened, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerUnlockGenerator)]
        public async void OnUnlockingGenerator(Player ply, Scp079Generator generator)
        {
            if (Instance.Config.EventsToLog.PlayerUnlockingGenerator && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorUnlocked, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerUnlockingGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorUnlocked, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        // This dont exist
        /*
        public async void OnContaining(ContainingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ContainingScp106 && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106WasContained, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ContainingScp106)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106WasContained, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }*/

        [PluginEvent(ServerEventType.Scp106Stalking)]
        public async void OnCreatingPortal(Player ply, bool isActive)
        {
            if (Instance.Config.EventsToLog.Scp106StartStalking && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106StartStalking, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.Scp106StartStalking)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106StartStalking, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public async void OnChangingItem(Player ply, ushort oldItem, ushort newItem )
        {
            var newItembase =  ply.ReferenceHub.inventory.UserInventory.Items[newItem];
            
            if (Instance.Config.EventsToLog.ChangingPlayerItem && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ItemChanged, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.CurrentItem.ItemTypeId, newItembase.ItemTypeId))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ItemChanged, ply.Nickname, ply.UserId, ply.CurrentItem.ItemTypeId, newItembase.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp079GainExperience)]
        public async void OnGainingExperience(Player ply, int amount, Scp079HudTranslation reason)
        {
            if (Instance.Config.EventsToLog.GainingScp079Experience && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GainedExperience, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, amount, reason))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Experience)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GainedExperience, ply.Nickname, ply.UserId, ply.Role, amount, reason))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp079LevelUpTier)]
        public async void OnGainingLevel(Player ply, int tier)
        {
            if (Instance.Config.EventsToLog.GainingScp079Level && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GainedLevel, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, tier - 1, tier))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Level)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GainedLevel, ply.Nickname, ply.UserId, ply.Role, tier - 1, tier))).ConfigureAwait(false);

        }

        [PluginEvent(ServerEventType.PlayerLeft)]
        public async void OnLeft(Player ply)
        {
            if (ply is not null)
            {
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Disconnects, string.Format(Language.Disconnect, ply.Nickname, ply.UserId, ply.IpAddress))).ConfigureAwait(false);
            }
            
            if (Instance.Config.EventsToLog.PlayerLeft && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.LeftServer, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerLeft)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.LeftServer, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerReloadWeapon)]
        public async void OnReloadingWeapon(Player ply, Firearm firearm)
        {
            if (Instance.Config.EventsToLog.ReloadingPlayerWeapon && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Reloaded, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.CurrentItem.ItemTypeId, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ReloadingPlayerWeapon)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Reloaded, ply.Nickname, ply.UserId, ply.CurrentItem.ItemTypeId, ply.Role))).ConfigureAwait(false);
        }

        /* // Nope
        public async void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatingWarheadPanel && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.AccessedWarhead, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerActivatingWarheadPanel)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.AccessedWarhead, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }*/

        [PluginEvent(ServerEventType.PlayerInteractElevator)]
        public async void OnInteractingElevator(Player ply, ElevatorChamber elevator)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingElevator && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.CalledElevator, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingElevator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.CalledElevator, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerInteractLocker)]
        public async void OnInteractingLocker(Player ply, Locker locker, LockerChamber chamber, bool canOpen)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingLocker && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.UsedLocker, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingLocker)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.UsedLocker, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }
        
        /*public async void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerTriggeringTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerTriggeringTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }*/

        [PluginEvent(ServerEventType.PlayerCloseGenerator)]
        public async void OnClosingGenerator(Player ply, Scp079Generator generator)
        {
            if (Instance.Config.EventsToLog.PlayerClosingGenerator && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorClosed, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerClosingGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorClosed, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerDeactivatedGenerator)]
        public async void OnStoppingGenerator(Player ply, Scp079Generator generator)
        {
            if (Instance.Config.EventsToLog.PlayerEjectingGeneratorTablet && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorEjected, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEjectingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorEjected, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerInteractDoor)]
        public async void OnInteractingDoor(Player ply, DoorVariant door, bool canOpen)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingDoor && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(door.IsConsideredOpen() ? Language.HasClosedADoor : Language.HasOpenedADoor, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, door.name))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingDoor)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(door.IsConsideredOpen() ? Language.HasClosedADoor : Language.HasOpenedADoor, ply.Nickname, ply.UserId, ply.Role, door.name))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp914Activate)]
        public async void OnActivatingScp914(Player ply, Scp914KnobSetting knobSetting)
        {
            if (Instance.Config.EventsToLog.ActivatingScp914 && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914HasBeenActivated, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, knobSetting))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ActivatingScp914)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp914HasBeenActivated, ply.Nickname, ply.UserId, ply.Role, knobSetting))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp914KnobChange)]
        public async void OnChangingScp914KnobSetting(Player ply, Scp914KnobSetting knobSetting, Scp914KnobSetting previousKnobSetting)
        {
            if (Instance.Config.EventsToLog.ChangingScp914KnobSetting && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914KnobSettingChanged, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, knobSetting))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ChangingScp914KnobSetting)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp914KnobSettingChanged, ply.Nickname, ply.UserId, ply.Role, knobSetting))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerEnterPocketDimension)]
        public async void OnEnteringPocketDimension(Player ply)
        {
            if (Instance.Config.EventsToLog.PlayerEnteringPocketDimension && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasEnteredPocketDimension, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEnteringPocketDimension)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasEnteredPocketDimension, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }
        
        [PluginEvent(ServerEventType.PlayerExitPocketDimension)]
        public async void OnEscapingPocketDimension(Player ply, bool isSuccessful)
        {
            if (Instance.Config.EventsToLog.PlayerEscapingPocketDimension && isSuccessful && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasEscapedPocketDimension, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEscapingPocketDimension && isSuccessful)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasEscapedPocketDimension, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp106TeleportPlayer)]
        public async void OnScp106TeleportPlayer(Player scp106, Player target)
        {
            if (Instance.Config.EventsToLog.Scp106Teleporting && (!scp106.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106TeleportPlayer, scp106.Nickname, Instance.Config.ShouldLogUserIds ? scp106.UserId : Language.Redacted, target.Nickname, Instance.Config.ShouldLogUserIds ? target.UserId : Language.Redacted))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.Scp106Teleporting)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106TeleportPlayer, scp106.Nickname, scp106.UserId, target.Nickname, target.UserId))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp079UseTesla)]
        public async void OnInteractingTesla(Player ply, TeslaGate tesla)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.Scp079InteractingTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public async void OnHurting(Player target, Player attacker, DamageHandlerBase damage)
        {
            if (Instance.Config.EventsToLog.HurtingPlayer && target != null && (attacker == null || !Instance.Config.ShouldLogFriendlyFireDamageOnly || attacker.Role.GetTeam() == target.Role.GetTeam()) && (!Instance.Config.ShouldRespectDoNotTrack || (attacker == null || (!attacker.DoNotTrack && !target.DoNotTrack))) && !Instance.Config.BlacklistedDamageTypes.Contains(Extensions.GetDamageType(damage)) && (!Instance.Config.OnlyLogPlayerDamage || attacker != null))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasDamagedForWith, attacker != null ? attacker.Nickname : "Server", Instance.Config.ShouldLogUserIds ? attacker != null ? attacker.UserId : string.Empty : Language.Redacted, attacker?.Role ?? RoleTypeId.None, target.Nickname, Instance.Config.ShouldLogUserIds ? target.UserId : Language.Redacted, target.Role, "NA", Extensions.GetDamageType(damage)))).ConfigureAwait(false);

            if (Instance.Config.StaffOnlyEventsToLog.HurtingPlayer && target != null && (attacker == null || !Instance.Config.ShouldLogFriendlyFireDamageOnly || attacker.Role.GetTeam() == target.Role.GetTeam()) && !Instance.Config.BlacklistedDamageTypes.Contains(Extensions.GetDamageType(damage)) && (!Instance.Config.OnlyLogPlayerDamage || attacker != null))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasDamagedForWith, attacker != null ? attacker.Nickname : "Server", attacker != null ? attacker.UserId : string.Empty, attacker?.Role ?? RoleTypeId.None, target.Nickname, target.UserId, target.Role, "NA", Extensions.GetDamageType(damage)))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public async void OnDying(Player target, Player killer, DamageHandlerBase damage)
        {
            if (Instance.Config.EventsToLog.PlayerDying && target != null && (killer == null || !Instance.Config.ShouldLogFriendlyFireKillsOnly || killer.Role.GetTeam() == target.Role.GetTeam()) && (!Instance.Config.ShouldRespectDoNotTrack || (killer == null || (!killer.DoNotTrack && !target.DoNotTrack))))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasKilledWith, killer != null ? killer.Nickname : "Server", Instance.Config.ShouldLogUserIds ? killer != null ? killer.UserId : string.Empty : Language.Redacted, killer?.Role ?? RoleTypeId.None, target.Nickname, Instance.Config.ShouldLogUserIds ? target.UserId : Language.Redacted, target.Role, Extensions.GetDamageType(damage)))).ConfigureAwait(false);

            if (Instance.Config.StaffOnlyEventsToLog.PlayerDying && target != null && (killer == null || !Instance.Config.ShouldLogFriendlyFireKillsOnly || killer.Role.GetTeam() == target.Role.GetTeam()))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasKilledWith, killer != null ? killer.Nickname : "Server", killer != null ? killer.UserId : string.Empty, killer?.Role ?? RoleTypeId.None, target.Nickname, target.UserId, target.Role, Extensions.GetDamageType(damage)))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerThrowItem)]
        public async void OnThrowingGrenade(Player ply, ItemBase item, Rigidbody rigidbody)
        {
            if (ply != null && Instance.Config.EventsToLog.PlayerThrowingGrenade && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ThrewAGrenade, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, item.ItemTypeId))).ConfigureAwait(false);
            if (ply != null && Instance.Config.StaffOnlyEventsToLog.PlayerThrowingGrenade)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ThrewAGrenade, ply.Nickname, ply.UserId, ply.Role, item.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public async void OnUsedMedicalItem(Player ply, ItemBase item)
        {
            if (ply != null && Instance.Config.EventsToLog.PlayerUsedMedicalItem && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.UsedMedicalItem, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, item.ItemTypeId))).ConfigureAwait(false);
            if (ply != null && Instance.Config.StaffOnlyEventsToLog.PlayerUsedMedicalItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.UsedMedicalItem, ply.Nickname, ply.UserId, ply.Role, item.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public async void OnChangingRole(Player player, PlayerRoleBase newRole, RoleTypeId oldRole, RoleChangeReason reason)
        {
            if (player != null && Instance.Config.EventsToLog.ChangingPlayerRole && (!player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ChangedRole, player.Nickname, Instance.Config.ShouldLogUserIds ? player.UserId : Language.Redacted, player.Role, newRole.RoleTypeId))).ConfigureAwait(false);
            if (player != null && Instance.Config.StaffOnlyEventsToLog.ChangingPlayerRole)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ChangedRole, player.Nickname, player.UserId, player.Role, newRole.RoleTypeId))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerJoined)]
        public async void OnVerified(Player ply)
        {
            if (Instance.Config.UseWatchlist)
            {
                if (DatabaseHandler.CheckWatchlist(ply.UserId, out string reason))
                    if (!string.IsNullOrEmpty(reason))
                        await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Watchlist, string.Format(Language.WatchlistedUserJoined, ply.Nickname, ply.UserId, ply.IpAddress, reason)));
            }

            if (Instance.Config.EventsToLog.PlayerJoined && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasJoinedTheGame, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, Instance.Config.ShouldLogIPAddresses ? ply.IpAddress : Language.Redacted))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerJoined)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasJoinedTheGame, ply.Nickname, ply.UserId, ply.IpAddress))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerRemoveHandcuffs)]
        public async void OnRemovingHandcuffs(Player player, Player target, bool state)
        {
            if (Instance.Config.EventsToLog.PlayerRemovingHandcuffs && ((!player.DoNotTrack && !target.DoNotTrack) || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasBeenFreedBy, target.Nickname, Instance.Config.ShouldLogUserIds ? target.UserId : Language.Redacted, target.Role, player.Nickname, Instance.Config.ShouldLogUserIds ? player.UserId : Language.Redacted, player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerRemovingHandcuffs)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasBeenFreedBy, target.Nickname, target.UserId, target.Role, player.Nickname, player.UserId, player.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerHandcuff)]
        public async void OnHandcuffing(Player cuffer, Player target)
        {
            if (Instance.Config.EventsToLog.HandcuffingPlayer && ((!cuffer.DoNotTrack && !target.DoNotTrack) || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasBeenHandcuffedBy, target.Nickname, Instance.Config.ShouldLogUserIds ? target.UserId : Language.Redacted, target.Role, cuffer.Nickname, Instance.Config.ShouldLogUserIds ? cuffer.UserId : Language.Redacted, cuffer.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.HandcuffingPlayer)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasBeenHandcuffedBy, target.Nickname, target.UserId, target.Role, cuffer.Nickname, cuffer.UserId, cuffer.Role))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.PlayerKicked)]
        public async void OnKicked(Player target, ICommandSender issuer,  string reason)
        {

            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, "kicks", string.Format(Language.WasKicked, target?.Nickname ?? Language.NotAuthenticated, target?.UserId ?? Language.NotAuthenticated, reason))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnBanned(PlayerBannedEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
            {
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Bans, string.Format(Language.WasBannedBy, ev.Player.Nickname, ev.Player.UserId, ev.Issuer.LogName, ev.Reason, DateTime.Now.AddSeconds(ev.Duration)))).ConfigureAwait(false);
            }
        }

        [PluginEvent(ServerEventType.BanIssued)]
        public async void OnOfflineBan(BanDetails banDetails, BanHandler.BanType banType)
        {
            if (Instance.Config.EventsToLog.OfflineBan)
            {
                var date = new DateTime(banDetails.Expires);
                
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Bans, string.Format(Language.BannIssued, banDetails.Id, banDetails.Issuer, banDetails.Reason, date.ToLocalTime()))).ConfigureAwait(false);
            }
        }

        /*
        [PluginEvent(ServerEventType.Intercom)]
        public async void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerIntercomSpeaking && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerIntercomSpeaking)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }*/

        /*
        [PluginEvent(ServerEventType.PickupItem)]
        public async void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerPickingupItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasPickedUp, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Pickup.Type))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerPickingupItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasPickedUp, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Pickup.Type))).ConfigureAwait(false);
        }*/

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public async void OnItemDropped(Player ply, ItemBase item)
        {
            if (Instance.Config.EventsToLog.PlayerItemDropped && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasDropped, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role, item.ItemTypeId))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerItemDropped)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasDropped, ply.Nickname, ply.UserId, ply.Role, item.ItemTypeId))).ConfigureAwait(false);
        }

        /*
        [PluginEvent(ServerEventType.Gro)]
        public async void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerGroup && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GroupSet, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.ChangingPlayerGroup)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GroupSet, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
        }*/
    }
}