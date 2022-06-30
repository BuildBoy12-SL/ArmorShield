// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Exiled.Events.EventArgs;
using MEC;

namespace ArmorShield
{
    using System.Collections.Generic;
    using ArmorShield.Events.EventArgs;
    using ArmorShield.Models;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using InventorySystem.Items.Armor;
    using PlayerStatsSystem;
    using AdvancedHints.Enums;
    using AdvancedHints;

    /// <summary>
    /// General event handlers.
    /// </summary>
    public class EventHandlers
    {
        private readonly Dictionary<ushort, AhpStat.AhpProcess> ahpProcesses = new();
        private readonly Plugin plugin;
        private CoroutineHandle coroutine;
        private Dictionary<Player, CoroutineHandle> ActiveCoroutineHandles = new Dictionary<Player, CoroutineHandle>();
        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        /// <param name="plugin">The <see cref="Plugin{TConfig}"/> class reference.</param>
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Events.Handlers.Player.OnItemAdded"/>
        public void OnItemAdded(ItemAddedEventArgs ev)
        {
            if (TryGetBodyArmor(ev.Player, out BodyArmor bodyArmor))
                UpdateShield(ev.Player, bodyArmor);
        }

        /// <inheritdoc cref="Events.Handlers.Player.OnRemovingItem"/>
        public void OnRemovingItem(RemovingItemEventArgs ev)
        {
            if (ev.Item is null)
                return;

            if (ahpProcesses.TryGetValue(ev.Item.Serial, out AhpStat.AhpProcess ahpProcess))
            {
                ev.Player.ReferenceHub.playerStats.GetModule<AhpStat>().ServerKillProcess(ahpProcess.KillCode);
                ahpProcesses.Remove(ev.Item.Serial);
                if (ActiveCoroutineHandles.TryGetValue(ev.Player, out coroutine))
                {
                    Timing.KillCoroutines(coroutine);
                    ev.Player.ShowHint("",0.01f); //Intended to clear the timer hint.
                }
            }

            if (TryGetBodyArmor(ev.Player, out BodyArmor bodyArmor, ev.Item))
                UpdateShield(ev.Player, bodyArmor);
        }

        public void OnAnyPlayerDamaged(ReferenceHub target, DamageHandlerBase handler)
        {
            Player player = Player.Get(target);
            if (!TryGetBodyArmor(player, out BodyArmor bodyArmor) ||
                !ahpProcesses.TryGetValue(bodyArmor.ItemSerial, out AhpStat.AhpProcess ahpProcess) ||
                !plugin.Config.ArmorShields.TryGetValue(bodyArmor.ItemTypeId, out ConfiguredAhp configuredAhp))
                return;
            ahpProcess.SustainTime = configuredAhp.Sustain;
            if (ActiveCoroutineHandles.TryGetValue(player, out coroutine))
            {
                Timing.KillCoroutines(coroutine);
                ActiveCoroutineHandles.Remove(player);
            }

            coroutine = Timing.RunCoroutine(RegenTimer(player, configuredAhp.Sustain));
            ActiveCoroutineHandles.Add(player, coroutine);
        }

        public void OnLeave(LeftEventArgs ev)
        {
            if (ActiveCoroutineHandles.TryGetValue(ev.Player, out coroutine))
            {
                ActiveCoroutineHandles.Remove(ev.Player);
                Timing.KillCoroutines(coroutine);
            }
        }

        private static bool TryGetBodyArmor(Player player, out BodyArmor armor, Item toExclude = null)
        {
            foreach (Item item in player.Items)
            {
                if (toExclude != item && item.Base is BodyArmor bodyArmor)
                {
                    armor = bodyArmor;
                    return true;
                }
            }

            armor = null;
            return false;
        }

        private void UpdateShield(Player player, BodyArmor armor)
        {
            if (!ahpProcesses.ContainsKey(armor.ItemSerial) && plugin.Config.ArmorShields.TryGetValue(armor.ItemTypeId, out ConfiguredAhp configuredAhp))
                ahpProcesses.Add(armor.ItemSerial, configuredAhp.AddTo(player));
        }

        public IEnumerator<float> RegenTimer(Player player, float RegenTime)
        {
            for (;;)
            {

                if (RegenTime <= 0 || player.Role.Type == RoleType.Spectator)
                {
                    ActiveCoroutineHandles.Remove(player);
                    player.ShowManagedHint("<align=left>Regenerating!</align>", 1f,true, DisplayLocation.Bottom);
                    yield break;
                }

                player.ShowManagedHint($"<align=left> AHP Regenerating in {RegenTime.ToString()}s</align>", 1f, true, DisplayLocation.Bottom);
                RegenTime -= 1f;
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
