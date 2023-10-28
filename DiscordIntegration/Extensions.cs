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

namespace DiscordIntegration
{
    using System.Text.RegularExpressions;
    using API.Commands;
    using CommandSystem;
    using PlayerRoles.PlayableScps.Scp3114;

    /// <summary>
    /// Useful Extension methods.
    /// </summary>
    public static class Extensions
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
        public static bool IsValidDiscordRoleId(this string discordRoleId) => IsValidDiscordId(discordRoleId);

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
        
        // Yes is copy from Exiled
        private static readonly Dictionary<byte, DamageType> TranslationIdConversionInternal = new()
        {
            { DeathTranslations.Asphyxiated.Id, DamageType.Asphyxiation },
            { DeathTranslations.Bleeding.Id, DamageType.Bleeding },
            { DeathTranslations.Crushed.Id, DamageType.Crushed },
            { DeathTranslations.Decontamination.Id, DamageType.Decontamination },
            { DeathTranslations.Explosion.Id, DamageType.Explosion },
            { DeathTranslations.Falldown.Id, DamageType.Falldown },
            { DeathTranslations.Poisoned.Id, DamageType.Poison },
            { DeathTranslations.Recontained.Id, DamageType.Recontainment },
            { DeathTranslations.Scp049.Id, DamageType.Scp049 },
            { DeathTranslations.Scp096.Id, DamageType.Scp096 },
            { DeathTranslations.Scp173.Id, DamageType.Scp173 },
            { DeathTranslations.Scp207.Id, DamageType.Scp207 },
            { DeathTranslations.Scp939Lunge.Id, DamageType.Scp939 },
            { DeathTranslations.Scp939Other.Id, DamageType.Scp939 },
            { DeathTranslations.Tesla.Id, DamageType.Tesla },
            { DeathTranslations.Unknown.Id, DamageType.Unknown },
            { DeathTranslations.Warhead.Id, DamageType.Warhead },
            { DeathTranslations.Zombie.Id, DamageType.Scp0492 },
            { DeathTranslations.BulletWounds.Id, DamageType.Firearm },
            { DeathTranslations.PocketDecay.Id, DamageType.PocketDimension },
            { DeathTranslations.SeveredHands.Id, DamageType.SeveredHands },
            { DeathTranslations.FriendlyFireDetector.Id, DamageType.FriendlyFireDetector },
            { DeathTranslations.UsedAs106Bait.Id, DamageType.FemurBreaker },
            { DeathTranslations.MicroHID.Id, DamageType.MicroHid },
            { DeathTranslations.Hypothermia.Id, DamageType.Hypothermia },
            { DeathTranslations.Scp3114Slap.Id, DamageType.Scp3114 },
            { DeathTranslations.MarshmallowMan.Id, DamageType.MarshmallowMan },
            { DeathTranslations.MetalPipe.Id, DamageType.MetalPipe },
        };

        private static readonly Dictionary<DeathTranslation, DamageType> TranslationConversionInternal = new()
        {
            { DeathTranslations.Asphyxiated, DamageType.Asphyxiation },
            { DeathTranslations.Bleeding, DamageType.Bleeding },
            { DeathTranslations.Crushed, DamageType.Crushed },
            { DeathTranslations.Decontamination, DamageType.Decontamination },
            { DeathTranslations.Explosion, DamageType.Explosion },
            { DeathTranslations.Falldown, DamageType.Falldown },
            { DeathTranslations.Poisoned, DamageType.Poison },
            { DeathTranslations.Recontained, DamageType.Recontainment },
            { DeathTranslations.Scp049, DamageType.Scp049 },
            { DeathTranslations.Scp096, DamageType.Scp096 },
            { DeathTranslations.Scp173, DamageType.Scp173 },
            { DeathTranslations.Scp207, DamageType.Scp207 },
            { DeathTranslations.Scp939Lunge, DamageType.Scp939 },
            { DeathTranslations.Scp939Other, DamageType.Scp939 },
            { DeathTranslations.Scp3114Slap, DamageType.Scp3114 },
            { DeathTranslations.Tesla, DamageType.Tesla },
            { DeathTranslations.Unknown, DamageType.Unknown },
            { DeathTranslations.Warhead, DamageType.Warhead },
            { DeathTranslations.Zombie, DamageType.Scp0492 },
            { DeathTranslations.BulletWounds, DamageType.Firearm },
            { DeathTranslations.PocketDecay, DamageType.PocketDimension },
            { DeathTranslations.SeveredHands, DamageType.SeveredHands },
            { DeathTranslations.FriendlyFireDetector, DamageType.FriendlyFireDetector },
            { DeathTranslations.UsedAs106Bait, DamageType.FemurBreaker },
            { DeathTranslations.MicroHID, DamageType.MicroHid },
            { DeathTranslations.Hypothermia, DamageType.Hypothermia },
            // halloween update
            { DeathTranslations.Scp3114Slap, DamageType.Scp3114 },
            { DeathTranslations.MarshmallowMan, DamageType.MarshmallowMan },
            { DeathTranslations.MetalPipe, DamageType.MetalPipe },
        };

        private static readonly Dictionary<ItemType, DamageType> ItemConversionInternal = new()
        {
            { ItemType.GunCrossvec, DamageType.Crossvec },
            { ItemType.GunLogicer, DamageType.Logicer },
            { ItemType.GunRevolver, DamageType.Revolver },
            { ItemType.GunShotgun, DamageType.Shotgun },
            { ItemType.GunAK, DamageType.AK },
            { ItemType.GunCOM15, DamageType.Com15 },
            { ItemType.GunCom45, DamageType.Com45 },
            { ItemType.GunCOM18, DamageType.Com18 },
            { ItemType.GunFSP9, DamageType.Fsp9 },
            { ItemType.GunE11SR, DamageType.E11Sr },
            { ItemType.MicroHID, DamageType.MicroHid },
            { ItemType.ParticleDisruptor, DamageType.ParticleDisruptor },
        };
        
        /// <summary>
        /// Gets the <see cref="DamageType"/> of an <see cref="DamageHandlerBase"/>s.
        /// </summary>
        /// <param name="damageHandlerBase">The DamageHandler to convert.</param>
        /// <returns>The <see cref="DamageType"/> of the <see cref="DamageHandlerBase"/>.</returns>
        public static DamageType GetDamageType(DamageHandlerBase damageHandlerBase)
        {
            switch (damageHandlerBase)
            {
                case CustomReasonDamageHandler:
                    return DamageType.Custom;
                case WarheadDamageHandler:
                    return DamageType.Warhead;
                case ExplosionDamageHandler:
                    return DamageType.Explosion;
                case Scp018DamageHandler:
                    return DamageType.Scp018;
                case RecontainmentDamageHandler:
                    return DamageType.Recontainment;
                case Scp096DamageHandler:
                    return DamageType.Scp096;
                case MicroHidDamageHandler:
                    return DamageType.MicroHid;
                case DisruptorDamageHandler:
                    return DamageType.ParticleDisruptor;
                case FirearmDamageHandler firearmDamageHandler:
                    return ItemConversionInternal.ContainsKey(firearmDamageHandler.WeaponType) ? ItemConversionInternal[firearmDamageHandler.WeaponType] : DamageType.Firearm;

                case ScpDamageHandler scpDamageHandler:
                    {
                        DeathTranslation translation = DeathTranslations.TranslationsById[scpDamageHandler._translationId];
                        if (translation.Id == DeathTranslations.PocketDecay.Id)
                            return DamageType.Scp106;

                        return TranslationIdConversionInternal.ContainsKey(translation.Id)
                            ? TranslationIdConversionInternal[translation.Id]
                            : DamageType.Scp;
                    }
                case UniversalDamageHandler universal:
                    {
                        DeathTranslation translation = DeathTranslations.TranslationsById[universal.TranslationId];

                        if (TranslationIdConversionInternal.ContainsKey(translation.Id))
                            return TranslationIdConversionInternal[translation.Id];

                        Log.Warning($"{nameof(Extensions)}.{nameof(damageHandlerBase)}: No matching {nameof(DamageType)} for {nameof(UniversalDamageHandler)} with ID {translation.Id}, type will be reported as {DamageType.Unknown}.");
                        return DamageType.Unknown;
                    }
            }

            return DamageType.Unknown;
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
        /// <see cref="Asphyxiated"/>.
        /// </summary>
        Asphyxiation,

        /// <summary>
        /// <see cref="Poisoned"/>.
        /// </summary>
        Poison,

        /// <summary>
        /// <see cref="CustomPlayerEffects.Bleeding"/>.
        /// </summary>
        Bleeding,

        /// <summary>
        /// Damage dealt by a <see cref="InventorySystem.Items.Firearms.Firearm"/> when the <see cref="ItemType"/> used is not available.
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
        /// <see cref="RespawnEffectsController.EffectType.Scp207"/>.
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
        /// <see cref="InventorySystem.Items.Usables.Scp244.Hypothermia.Hypothermia"/>.
        /// </summary>
        Hypothermia,

        /// <summary>
        /// Damage caused by <see cref="ItemType.ParticleDisruptor"/>.
        /// </summary>
        ParticleDisruptor,

        /// <summary>
        /// Damage caused by SCP-956.
        /// </summary>
        Scp956,

        /// <summary>
        /// Damage caused by <see cref="ItemType.GunCom45"/>.
        /// </summary>
        Com45,

        /// <summary>
        /// Dmage caused by <see cref="RoleTypeId.Scp3114"/>
        /// </summary>
        Scp3114,

        /// <summary>
        /// Damage caused by a Marshmallow Man.
        /// </summary>
        MarshmallowMan,

        /// <summary>
        /// Damage caused by a Metal Pipe.
        /// </summary>
        MetalPipe,
    }
}
