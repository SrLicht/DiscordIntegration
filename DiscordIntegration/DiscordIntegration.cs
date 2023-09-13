// -----------------------------------------------------------------------
// <copyright file="DiscordIntegration.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using PluginAPI.Core;
using PluginAPI.Core.Attributes;

namespace DiscordIntegration
{
    using System;
    using System.Threading;
    using API;
    using API.Configs;
    using Events;
    using HarmonyLib;
    using Version = System.Version;

    /// <summary>
    /// Link a Discord server with an SCP: SL server.
    /// </summary>
    public class DiscordIntegration
    {
        private NetworkHandler networkHandler;

        private Harmony harmony;

        private int slots;

        /// <summary>
        /// Gets the plugin <see cref="Language"/> instance.
        /// </summary>
        public static Language Language { get; private set; }

        /// <summary>
        /// Gets the <see cref="API.Network"/> instance.
        /// </summary>
        public static Network Network { get; set; }

        /// <summary>
        /// Gets or sets the network <see cref="CancellationTokenSource"/> instance.
        /// </summary>
        public static CancellationTokenSource NetworkCancellationTokenSource { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DiscordIntegration"/> instance.
        /// </summary>
        public static DiscordIntegration Instance { get; private set; }

        [PluginConfig] public Config Config;

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

        /// <summary>
        /// Fired when the plugin is enabled.
        /// </summary>
        [PluginEntryPoint("DiscordIntegration", "1.0.0", 
            "server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the Discord bot.", "SrLicht")]
        public void OnEnabled()
        {
            Instance = this;
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
            Network = new Network(Config.Bot.IPAddress, Config.Bot.Port, TimeSpan.FromSeconds(Config.Bot.ReconnectionInterval));
            
            NetworkCancellationTokenSource = new CancellationTokenSource();
            
            Language.Save();
            Language.Load();
            RegisterEvents();
            Bot.UpdateActivityCancellationTokenSource = new CancellationTokenSource();
            Bot.UpdateChannelsTopicCancellationTokenSource = new CancellationTokenSource();
            
            _ = Network.Start(NetworkCancellationTokenSource);
            
            _ = Bot.UpdateActivity(Bot.UpdateActivityCancellationTokenSource.Token);
            _ = Bot.UpdateChannelsTopic(Bot.UpdateChannelsTopicCancellationTokenSource.Token);
            
            Log.Debug($"Path of language is {Language.FullPath}");
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

        [PluginReload]
        public void OnReloaded()
        {
            OnDisabled();
            OnEnabled();
        }

        private void RegisterEvents()
        {

            networkHandler = new NetworkHandler();
            
            PluginAPI.Events.EventManager.RegisterEvents(this, new Events.MapHandler(Instance));
            PluginAPI.Events.EventManager.RegisterEvents(this, new Events.PlayerHandler(Instance));
            PluginAPI.Events.EventManager.RegisterEvents(this, new Events.ServerHandler(Instance));
            PluginAPI.Events.EventManager.RegisterEvents(this, new Events.VeryHelpful(Instance));

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

        public void DisconnectNetwork()
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
        }

        public void ReconnectNetwork()
        {
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
            PluginAPI.Events.EventManager.UnregisterEvents<Events.MapHandler>(this);
            PluginAPI.Events.EventManager.UnregisterEvents<Events.NetworkHandler>(this);
            PluginAPI.Events.EventManager.UnregisterEvents<Events.PlayerHandler>(this);
            PluginAPI.Events.EventManager.UnregisterEvents<Events.ServerHandler>(this);
            PluginAPI.Events.EventManager.UnregisterEvents<Events.VeryHelpful>(this);
            
            Network.SendingError -= networkHandler.OnSendingError;
            Network.ReceivingError -= networkHandler.OnReceivingError;
            Network.UpdatingConnectionError -= networkHandler.OnUpdatingConnectionError;
            Network.ConnectingError -= networkHandler.OnConnectingError;
            Network.Connected -= networkHandler.OnConnected;
            Network.Connecting -= networkHandler.OnConnecting;
            Network.ReceivedFull -= networkHandler.OnReceivedFull;
            Network.Sent -= networkHandler.OnSent;
            Network.Terminated -= networkHandler.OnTerminated;
            
            networkHandler = null;
        }
    }
}
