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
    using System.Linq;
    using Dependency;
    using global::DiscordIntegration.Extensions.Extensions;
    using PluginAPI.Events;
    using static DiscordIntegration;
    using static UnityEngine.GraphicsBuffer;

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
        [PluginEvent]
        public async void OnActivateGenerator(PlayerActivateGeneratorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatedGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorInserted, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerActivatedGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorInserted, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }
        

        [PluginEvent]
        public async void OnOpeningGenerator(PlayerOpenGeneratorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerOpeningGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorOpened, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);

            if (Instance.Config.StaffOnlyEventsToLog.PlayerOpeningGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorOpened, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnUnlockingGenerator(PlayerUnlockGeneratorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerUnlockingGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerUnlockingGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
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

        [PluginEvent]
        public async void OnStalking(Scp106StalkingEvent ev)
        {
            if (Instance.Config.EventsToLog.Scp106StartStalking && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106StartStalking, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.Scp106StartStalking)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106StartStalking, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnChangingItem(PlayerChangeItemEvent ev)
        {
            var newItembase = ev.Player.Items.FirstOrDefault(p => p.ItemSerial == ev.NewItem);

            if (newItembase is null)
                return;

            if (Instance.Config.EventsToLog.ChangingPlayerItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ItemChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.CurrentItem.ItemTypeId, newItembase.ItemTypeId))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ItemChanged, ev.Player.Nickname, ev.Player.UserId, ev.Player.CurrentItem.ItemTypeId, newItembase.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnGainingExperience(Scp079GainExperienceEvent ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Experience && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GainedExperience, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Amount, ev.Reason))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Experience)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GainedExperience, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Amount, ev.Reason))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnGainingLevel(Scp079LevelUpTierEvent ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Level && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GainedLevel, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Tier - 1, ev.Tier))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Level)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GainedLevel, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Tier - 1, ev.Tier))).ConfigureAwait(false);

        }

        [PluginEvent]
        public async void OnLeft(PlayerLeftEvent ev)
        {
            if (ev.Player is not null)
            {
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Disconnects, string.Format(Language.Disconnect, ev.Player.Nickname, ev.Player.UserId, ev.Player.IpAddress))).ConfigureAwait(false);
            }
            
            if (Instance.Config.EventsToLog.PlayerLeft && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.LeftServer, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerLeft)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.LeftServer, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnReloadingWeapon(PlayerReloadWeaponEvent ev)
        {
            if (Instance.Config.EventsToLog.ReloadingPlayerWeapon && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Reloaded, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.CurrentItem.ItemTypeId, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.ReloadingPlayerWeapon)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Reloaded, ev.Player.Nickname, ev.Player.UserId, ev.Player.CurrentItem.ItemTypeId, ev.Player.Role))).ConfigureAwait(false);
        }

        /* // Nope
        public async void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatingWarheadPanel && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.AccessedWarhead, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerActivatingWarheadPanel)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.AccessedWarhead, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }*/

        [PluginEvent]
        public async void OnInteractingElevator(PlayerInteractElevatorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingElevator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.CalledElevator, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingElevator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.CalledElevator, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnInteractingLocker(PlayerInteractLockerEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingLocker && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.UsedLocker, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingLocker)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.UsedLocker, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        /*public async void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerTriggeringTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerTriggeringTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }*/

        [PluginEvent]
        public async void OnClosingGenerator(PlayerCloseGeneratorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerClosingGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorClosed, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerClosingGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorClosed, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnStoppingGenerator(PlayerDeactivatedGeneratorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerEjectingGeneratorTablet && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorEjected, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEjectingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorEjected, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnInteractingDoor(PlayerInteractDoorEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingDoor && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(ev.Door.IsConsideredOpen() ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Door.name))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingDoor)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(ev.Door.IsConsideredOpen() ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Door.name))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnActivatingScp914(Scp914ActivateEvent ev)
        {
            if (Instance.Config.EventsToLog.ActivatingScp914 && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.KnobSetting))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.ActivatingScp914)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.KnobSetting))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnChangingScp914KnobSetting(Scp914KnobChangeEvent ev)
        {
            if (Instance.Config.EventsToLog.ChangingScp914KnobSetting && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.KnobSetting))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.ChangingScp914KnobSetting)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.KnobSetting))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnEnteringPocketDimension(PlayerEnterPocketDimensionEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerEnteringPocketDimension && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEnteringPocketDimension)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnEscapingPocketDimension(PlayerExitPocketDimensionEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerEscapingPocketDimension && ev.IsSuccessful && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEscapingPocketDimension && ev.IsSuccessful)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnScp106TeleportPlayer(Scp106TeleportPlayerEvent ev)
        {
            if (Instance.Config.EventsToLog.Scp106Teleporting && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106TeleportPlayer, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.Scp106Teleporting)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106TeleportPlayer, ev.Player.Nickname, ev.Player.UserId, ev.Target.Nickname, ev.Target.UserId))).ConfigureAwait(false);
        }

        [PluginEvent(ServerEventType.Scp079UseTesla)]
        public async void OnInteractingTesla(Player ply, TeslaGate tesla)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla && (!ply.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ply.Nickname, Instance.Config.ShouldLogUserIds ? ply.UserId : Language.Redacted, ply.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.Scp079InteractingTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ply.Nickname, ply.UserId, ply.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnInteractingTesla(Scp079UseTeslaEvent ev)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.Scp079InteractingTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnHurting(PlayerDamageEvent ev)
        {
            if (Instance.Config.EventsToLog.HurtingPlayer && ev.Target != null && (ev.Player == null || !Instance.Config.ShouldLogFriendlyFireDamageOnly || ev.Player.Role.GetTeam() == ev.Target.Role.GetTeam()) && (!Instance.Config.ShouldRespectDoNotTrack || (ev.Player == null || (!ev.Player.DoNotTrack && !ev.Target.DoNotTrack))) && !Instance.Config.BlacklistedDamageTypes.Contains(DamageTypeExtensions.GetDamageType(ev.DamageHandler)) && (!Instance.Config.OnlyLogPlayerDamage || ev.Player != null))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasDamagedForWith, ev.Player != null ? ev.Player.Nickname : "Server", Instance.Config.ShouldLogUserIds ? ev.Player != null ? ev.Player.UserId : string.Empty : Language.Redacted, ev.Player?.Role ?? RoleTypeId.None, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, "NA", DamageTypeExtensions.GetDamageType(ev.DamageHandler)))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.HurtingPlayer && ev.Target != null && (ev.Player == null || !Instance.Config.ShouldLogFriendlyFireDamageOnly || ev.Player.Role.GetTeam() == ev.Target.Role.GetTeam()) && !Instance.Config.BlacklistedDamageTypes.Contains(DamageTypeExtensions.GetDamageType(ev.DamageHandler)) && (!Instance.Config.OnlyLogPlayerDamage || ev.Player != null))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasDamagedForWith, ev.Player != null ? ev.Player.Nickname : "Server", ev.Player != null ? ev.Player.UserId : string.Empty, ev.Player?.Role ?? RoleTypeId.None, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, "NA", DamageTypeExtensions.GetDamageType(ev.DamageHandler)))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnDying(PlayerDyingEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerDying && ev.Player != null && (ev.Attacker == null || !Instance.Config.ShouldLogFriendlyFireKillsOnly || ev.Attacker.Team == ev.Player.Team) && (!Instance.Config.ShouldRespectDoNotTrack || (ev.Attacker == null || (!ev.Attacker.DoNotTrack && !ev.Player.DoNotTrack))))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasKilledWith, ev.Attacker != null ? ev.Attacker.Nickname : "Server", Instance.Config.ShouldLogUserIds ? ev.Attacker != null ? ev.Attacker.UserId : string.Empty : Language.Redacted, ev.Attacker?.Role ?? RoleTypeId.None, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, DamageTypeExtensions.GetDamageType(ev.DamageHandler)))).ConfigureAwait(false);

            if (Instance.Config.StaffOnlyEventsToLog.PlayerDying && ev.Player != null && (ev.Attacker == null || !Instance.Config.ShouldLogFriendlyFireKillsOnly || ev.Attacker.Team == ev.Player.Team))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasKilledWith, ev.Attacker != null ? ev.Attacker.Nickname : "Server", ev.Attacker != null ? ev.Attacker.UserId : string.Empty, ev.Attacker?.Role ?? RoleTypeId.None, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, DamageTypeExtensions.GetDamageType(ev.DamageHandler)))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnThrowingProjectile(PlayerThrowProjectileEvent ev)
        {
            if (ev.Thrower != null && Instance.Config.EventsToLog.PlayerThrowingGrenade && (!ev.Thrower.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ThrewAGrenade, ev.Thrower.Nickname, Instance.Config.ShouldLogUserIds ? ev.Thrower.UserId : Language.Redacted, ev.Thrower.Role, ev.Item.ItemTypeId))).ConfigureAwait(false);
            
            if (ev.Thrower != null && Instance.Config.StaffOnlyEventsToLog.PlayerThrowingGrenade)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ThrewAGrenade, ev.Thrower.Nickname, ev.Thrower.UserId, ev.Thrower.Role, ev.Item.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnUsedMedicalItem(PlayerUsedItemEvent ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerUsedMedicalItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.UsedMedicalItem, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Item.ItemTypeId))).ConfigureAwait(false);
            
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerUsedMedicalItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.UsedMedicalItem, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Item.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnChangingRole(PlayerChangeRoleEvent ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerRole && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ChangedRole, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.NewRole))).ConfigureAwait(false);
            
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.ChangingPlayerRole)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ChangedRole, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.NewRole))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnJoined(PlayerJoinedEvent ev)
        {
            if (Instance.Config.UseWatchlist)
            {
                if (DatabaseHandler.CheckWatchlist(ev.Player.UserId, out string reason))
                    if (!string.IsNullOrEmpty(reason))
                        await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Watchlist, string.Format(Language.WatchlistedUserJoined, ev.Player.Nickname, ev.Player.UserId, ev.Player.IpAddress, reason)));
            }

            if (Instance.Config.EventsToLog.PlayerJoined && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, Instance.Config.ShouldLogIPAddresses ? ev.Player.IpAddress : Language.Redacted))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerJoined)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, ev.Player.UserId, ev.Player.IpAddress))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnRemovingHandcuffs(PlayerRemoveHandcuffsEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerRemovingHandcuffs && ((!ev.Player.DoNotTrack && !ev.Target.DoNotTrack) || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerRemovingHandcuffs)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnHandcuffing(PlayerHandcuffEvent ev)
        {
            if (Instance.Config.EventsToLog.HandcuffingPlayer && ((!ev.Player.DoNotTrack && !ev.Target.DoNotTrack) || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.HandcuffingPlayer)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnKicked(PlayerKickedEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, "kicks", string.Format(Language.WasKicked, ev.Player?.Nickname ?? Language.NotAuthenticated, ev.Player?.UserId ?? Language.NotAuthenticated, ev.Reason))).ConfigureAwait(false);
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

        [PluginEvent]
        public async void OnOfflineBan(BanIssuedEvent ev)
        {
            if (Instance.Config.EventsToLog.OfflineBan)
            {
                var date = new DateTime(ev.BanDetails.Expires).ToLocalTime();

                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Bans, string.Format(Language.BannIssued, ev.BanDetails.Id, ev.BanDetails.Issuer, ev.BanDetails.Reason, date))).ConfigureAwait(false);
            }
        }
        [PluginEvent]
        public async void OnIntercomSpeaking(PlayerUsingIntercomEvent ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerIntercomSpeaking && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerIntercomSpeaking)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnPickingUpItem(PlayerSearchedPickupEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerPickingupItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasPickedUp, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Item.Info.ItemId))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerPickingupItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasPickedUp, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Item.Info.ItemId))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnItemDropped(PlayerDropItemEvent ev)
        {
            if (Instance.Config.EventsToLog.PlayerItemDropped && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasDropped, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Item.ItemTypeId))).ConfigureAwait(false);
            
            if (Instance.Config.StaffOnlyEventsToLog.PlayerItemDropped)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasDropped, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Item.ItemTypeId))).ConfigureAwait(false);
        }

        [PluginEvent]
        public async void OnChangingGroup(PlayerGetGroupEvent ev)
        {

            /*if (Instance.Config.EventsToLog.ChangingPlayerGroup && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GroupSet, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.UserId : Language.Redacted, ev.Player.Role, ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);*/
            
            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerGroup)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GroupSet, "None", ev.UserId, "None", ev.Group?.BadgeText ?? Language.None, ev.Group?.BadgeColor ?? Language.None))).ConfigureAwait(false);
        }
    }
}