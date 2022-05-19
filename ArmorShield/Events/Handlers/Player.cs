// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ArmorShield.Events.Handlers
{
    using ArmorShield.Events.EventArgs;
    using Exiled.Events.Extensions;
    using InventorySystem;
    using InventorySystem.Items;
    using InventorySystem.Items.Pickups;
    using static Exiled.Events.Events;

    /// <summary>
    /// Player related events.
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Invoked after a <see cref="T:Exiled.API.Features.Player" /> has an item added to their inventory.
        /// </summary>
        public static event CustomEventHandler<ItemAddedEventArgs> ItemAdded;

        /// <summary>
        /// Invoked before a <see cref="T:Exiled.API.Features.Player" /> has an item removed from their inventory.
        /// </summary>
        public static event CustomEventHandler<RemovingItemEventArgs> RemovingItem;

        /// <summary>
        /// Called after a <see cref="T:Exiled.API.Features.Player" /> has an item added to their inventory.
        /// </summary>
        /// <param name="inventory">The <see cref="Inventory"/> the item was added to.</param>
        /// <param name="itemBase">The added <see cref="ItemBase"/>.</param>
        /// <param name="pickupBase">The <see cref="ItemPickupBase"/> the <see cref="ItemBase"/> originated from.</param>
        public static void OnItemAdded(Inventory inventory, ItemBase itemBase, ItemPickupBase pickupBase) => ItemAdded.InvokeSafely(new ItemAddedEventArgs(inventory, itemBase, pickupBase));

        /// <summary>
        /// Called before a <see cref="T:Exiled.API.Features.Player" /> has an item removed from their inventory.
        /// </summary>
        /// <param name="ev">The <see cref="RemovingItemEventArgs"/> instance.</param>
        public static void OnRemovingItem(RemovingItemEventArgs ev) => RemovingItem.InvokeSafely(ev);
    }
}