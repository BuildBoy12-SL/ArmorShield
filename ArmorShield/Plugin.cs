// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ArmorShield
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem;
    using PlayerStatsSystem;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;
        private EventHandlers eventHandlers;

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override string Name => "ArmorShield";

        /// <inheritdoc/>
        public override string Prefix => "ArmorShield";

        /// <inheritdoc/>
        public override Version Version { get; } = new(1, 0, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new(5, 2, 1);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            InventoryExtensions.OnItemAdded += Events.Handlers.Player.OnItemAdded;

            harmony = new Harmony($"armorShield.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            eventHandlers = new EventHandlers(this);
            Events.Handlers.Player.ItemAdded += eventHandlers.OnItemAdded;
            Events.Handlers.Player.RemovingItem += eventHandlers.OnRemovingItem;
            PlayerStats.OnAnyPlayerDamaged += eventHandlers.OnAnyPlayerDamaged;
            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Events.Handlers.Player.ItemAdded -= eventHandlers.OnItemAdded;
            Events.Handlers.Player.RemovingItem -= eventHandlers.OnRemovingItem;
            eventHandlers = null;

            harmony.UnpatchAll(harmony.Id);
            harmony = null;

            InventoryExtensions.OnItemAdded -= Events.Handlers.Player.OnItemAdded;
            base.OnDisabled();
        }
    }
}