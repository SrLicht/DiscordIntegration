// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using CustomPlayerEffects;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using Respawning;

namespace DiscordIntegration.Extensions
{
    using System.Text.RegularExpressions;
    using API.Commands;
    using CommandSystem;
    using PlayerRoles.PlayableScps.Scp3114;

    /// <summary>
    /// Useful Extension methods.
    /// </summary>
    public static class GeneralExtensions
    {
        /// <summary>
        /// Checks if a user ID is valid.
        /// </summary>
        /// <param name="userId">The user ID to be checked.</param>
        /// <returns>Returns a value indicating whether the user ID is valid or not.</returns>
        public static bool IsValidUserId(this string userId) => Regex.IsMatch(userId, "^([0-9]{17})@(steam|patreon|northwood)|([0-9]{18})@(discord)$");

        /// <summary>
        /// Checks if a Discord ID is valid.
        /// </summary>
        /// <param name="discordId">The Discord ID to be checked.</param>
        /// <returns>Returns a value indicating whether the Discord ID is valid or not.</returns>
        public static bool IsValidDiscordId(this string discordId) => Regex.IsMatch(discordId, "^[0-9]{18}$");

        /// <summary>
        /// Checks if a Discord role ID is valid.
        /// </summary>
        /// <param name="discordRoleId">The Discord role ID to be checked.</param>
        /// <returns>Returns a value indicating whether the Discord ID is valid or not.</returns>
        public static bool IsValidDiscordRoleId(this string discordRoleId) => discordRoleId.IsValidDiscordId();

        /// <summary>
        /// Gets a compatible and JSON serializable <see cref="CommandSender"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ICommandSender"/> to be checked.</param>
        /// <returns>Returns the compatible <see cref="CommandSender"/>.</returns>
        public static CommandSender GetCompatible(this ICommandSender sender) => ((CommandSender)sender).GetCompatible();

        /// <summary>
        /// Gets a compatible and JSON serializable <see cref="CommandSender"/>.
        /// </summary>
        /// <param name="sender">The sender to be checked.</param>
        /// <returns>Returns the compatible <see cref="CommandSender"/>.</returns>
        public static CommandSender GetCompatible(this CommandSender sender)
        {
            if (sender.GetType() != typeof(RemoteAdmin.PlayerCommandSender))
                return sender;

            return new PlayerCommandSender(sender.SenderId, sender.Nickname, sender.Permissions, sender.KickPower, sender.FullPermissions);
        }
    }

    public enum DamageType
    {
        /// <summary>
        /// Unknown damage source.
        /// </summary>
        Unknown,

        /// <summary>
        /// Fall damage.
        /// </summary>
        Falldown,

        /// <summary>
        /// Alpha Warhead.
        /// </summary>
        Warhead,

        /// <summary>
        /// LCZ Decontamination.
        /// </summary>
        Decontamination,

        /// <summary>
        /// <see cref="EffectType.Asphyxiated"/>.
        /// </summary>
        Asphyxiation,

        /// <summary>
        /// <see cref="EffectType.Poisoned"/>.
        /// </summary>
        Poison,

        /// <summary>
        /// <see cref="EffectType.Bleeding"/>.
        /// </summary>
        Bleeding,

        /// <summary>
        /// Damage dealt by a <see cref="Features.Items.Firearm"/> when the <see cref="ItemType"/> used is not available.
        /// </summary>
        Firearm,

        /// <summary>
        /// Damage dealt by a <see cref="Features.Items.MicroHid"/>.
        /// </summary>
        MicroHid,

        /// <summary>
        /// Damage dealt by a Tesla Gate.
        /// </summary>
        Tesla,

        /// <summary>
        /// Damage is dealt by a <see cref="Side.Scp"/> when the <see cref="RoleTypeId"/> used is not available.
        /// </summary>
        Scp,

        /// <summary>
        /// Damage dealt by frag grenades.
        /// </summary>
        Explosion,

        /// <summary>
        /// Damage dealt by SCP-018.
        /// </summary>
        Scp018,

        /// <summary>
        /// <see cref="EffectType.Scp207"/>.
        /// </summary>
        Scp207,

        /// <summary>
        /// Damage is dealt by SCP Recontainment procedure.
        /// </summary>
        Recontainment,

        /// <summary>
        /// Crushed by the checkpoint killer trigger.
        /// </summary>
        Crushed,

        /// <summary>
        /// Damage caused by the femur breaker.
        /// </summary>
        FemurBreaker,

        /// <summary>
        /// Damage caused by the pocket dimension.
        /// </summary>
        PocketDimension,

        /// <summary>
        /// Damage caused by the friendly fire detector.
        /// </summary>
        FriendlyFireDetector,

        /// <summary>
        /// Damage caused by severed hands.
        /// </summary>
        SeveredHands,

        /// <summary>
        /// Damage caused by a custom source.
        /// </summary>
        Custom,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp049"/>.
        /// </summary>
        Scp049,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp096"/>.
        /// </summary>
        Scp096,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp173"/>.
        /// </summary>
        Scp173,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp939"/>.
        /// </summary>
        Scp939,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp0492"/>.
        /// </summary>
        Scp0492,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp106"/>.
        /// </summary>
        Scp106,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunCrossvec"/>.
        /// </summary>
        Crossvec,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunLogicer"/>.
        /// </summary>
        Logicer,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunRevolver"/>.
        /// </summary>
        Revolver,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunShotgun"/>.
        /// </summary>
        Shotgun,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunAK"/>.
        /// </summary>
        AK,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunCOM15"/>.
        /// </summary>
        Com15,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunCOM18"/>.
        /// </summary>
        Com18,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunFSP9"/>.
        /// </summary>
        Fsp9,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunE11SR"/>.
        /// </summary>
        E11Sr,

        /// <summary>
        /// <see cref="EffectType.Hypothermia"/>.
        /// </summary>
        Hypothermia,

        /// <summary>
        /// Damage caused by <see cref="ItemType.ParticleDisruptor"/>.
        /// </summary>
        ParticleDisruptor,

        /// <summary>
        /// Damage caused by <see cref="EffectType.CardiacArrest"/>.
        /// </summary>
        CardiacArrest,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunCom45"/>.
        /// </summary>
        Com45,

        /// <summary>
        /// Damage caused by <see cref="ItemType.Jailbird"/>.
        /// </summary>
        Jailbird,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunFRMG0"/>.
        /// </summary>
        Frmg0,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunA7"/>.
        /// </summary>
        A7,

        /// <summary>
        /// Damage caused by <see cref="RoleTypeId.Scp3114"/>.
        /// </summary>
        Scp3114,

        /// <summary>
        /// <see cref="EffectType.Strangled"/>.
        /// </summary>
        Strangled,

#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
#pragma warning disable SA1602 // Enumeration items should be documented
        MarshmallowMan,
        Silent,
        MetalPipe,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    }
}
