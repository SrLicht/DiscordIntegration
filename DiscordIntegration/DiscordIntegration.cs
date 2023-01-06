// -----------------------------------------------------------------------
// <copyright file="DiscordIntegration.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using DiscordIntegration.Dependency.Database;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;

namespace DiscordIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using API;
    using API.Configs;
    using API.User;
    using Events;
    using HarmonyLib;
    using MEC;
    using Version = System.Version;

    /// <summary>
    /// Link a Discord server with an SCP: SL server.
    /// </summary>
    public class DiscordIntegration
    {
        private static readonly DiscordIntegration InstanceValue = new ();

        private MapHandler mapHandler;

        private ServerHandler serverHandler;

        private PlayerHandler playerHandler;

        private NetworkHandler networkHandler;

        private Harmony harmony;

        private int slots;

        private DiscordIntegration()
        {
        }

        /// <summary>
        /// Gets the plugin <see cref="Language"/> instance.
        /// </summary>
        public static Language Language { get; private set; }

        /// <summary>
        /// Gets the <see cref="API.Network"/> instance.
        /// </summary>
        public static Network Network { get; private set; }

        /// <summary>
        /// Gets or sets the network <see cref="CancellationTokenSource"/> instance.
        /// </summary>
        public static CancellationTokenSource NetworkCancellationTokenSource { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DiscordIntegration"/> instance.
        /// </summary>
        public static DiscordIntegration Instance => InstanceValue;

        /// <summary>
        /// Gets the server slots.
        /// </summary>
        public int Slots
        {
            get
            {
                if (Server.MaxPlayers > 0)
                    slots = Server.MaxPlayers;
                return slots;
            }
        }


        [PluginConfig] public Config Config;
        /// <summary>
        /// Fired when the plugin is enabled.
        /// </summary>
        [PluginEntryPoint("DiscordIntegration", "1.0.0", 
            "server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the Discord bot.", "SrLicht")]
        public void OnEnabled()
        {
            try
            {
                harmony = new Harmony($"com.joker.DI-{DateTime.Now.Ticks}");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }

            Language = new Language();
            Network = new Network(Instance.Config.Bot.IPAddress, Instance.Config.Bot.Port, TimeSpan.FromSeconds(Instance.Config.Bot.ReconnectionInterval));

            NetworkCancellationTokenSource = new CancellationTokenSource();

            Language.Save();
            Language.Load();

            DatabaseHandler.Init();
            
            RegisterEvents();

            Bot.UpdateActivityCancellationTokenSource = new CancellationTokenSource();
            Bot.UpdateChannelsTopicCancellationTokenSource = new CancellationTokenSource();

            _ = Network.Start(NetworkCancellationTokenSource);

            _ = Bot.UpdateActivity(Bot.UpdateActivityCancellationTokenSource.Token);
            _ = Bot.UpdateChannelsTopic(Bot.UpdateChannelsTopicCancellationTokenSource.Token);
        }

        /// <summary>
        /// Fired when the plugin is disabled.
        /// </summary>
        [PluginUnload]
        public void OnDisabled()
        {
            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            NetworkCancellationTokenSource.Cancel();
            NetworkCancellationTokenSource.Dispose();

            Network.Close();

            Bot.UpdateActivityCancellationTokenSource.Cancel();
            Bot.UpdateActivityCancellationTokenSource.Dispose();

            Bot.UpdateChannelsTopicCancellationTokenSource.Cancel();
            Bot.UpdateChannelsTopicCancellationTokenSource.Dispose();

            UnregisterEvents();

            Language = null;
            Network = null;
        }

        private void RegisterEvents()
        {
            mapHandler = new MapHandler();
            serverHandler = new ServerHandler();
            playerHandler = new PlayerHandler();
            networkHandler = new NetworkHandler();

            Network.SendingError += networkHandler.OnSendingError;
            Network.ReceivingError += networkHandler.OnReceivingError;
            Network.UpdatingConnectionError += networkHandler.OnUpdatingConnectionError;
            Network.ConnectingError += networkHandler.OnConnectingError;
            Network.Connected += networkHandler.OnConnected;
            Network.Connecting += networkHandler.OnConnecting;
            Network.ReceivedFull += networkHandler.OnReceivedFull;
            Network.Sent += networkHandler.OnSent;
            Network.Terminated += networkHandler.OnTerminated;
        }

        private void UnregisterEvents()
        {
            Network.SendingError -= networkHandler.OnSendingError;
            Network.ReceivingError -= networkHandler.OnReceivingError;
            Network.UpdatingConnectionError -= networkHandler.OnUpdatingConnectionError;
            Network.ConnectingError -= networkHandler.OnConnectingError;
            Network.Connected -= networkHandler.OnConnected;
            Network.Connecting -= networkHandler.OnConnecting;
            Network.ReceivedFull -= networkHandler.OnReceivedFull;
            Network.Sent -= networkHandler.OnSent;
            Network.Terminated -= networkHandler.OnTerminated;

            playerHandler = null;
            mapHandler = null;
            serverHandler = null;
            networkHandler = null;
        }
    }
}
