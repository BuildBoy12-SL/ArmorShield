// -----------------------------------------------------------------------
// <copyright file="RemovingItemEventArgs.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ArmorShield.Events.EventArgs
{
    using System;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using InventorySystem;
    using InventorySystem.Items;

    /// <summary>
    /// Contains all information before removing an item from a player's inventory.
    /// </summary>
    public class RemovingItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemovingItemEventArgs"/> class.
        /// </summary>
        /// <param name="inventory">The <see cref="Inventory"/> the item is being removed from.</param>
        /// <param name="serial">The serial of the <see cref="ItemBase"/> being removed.</param>
        public RemovingItemEventArgs(Inventory inventory, ushort serial)
        {
            Player = Player.Get(inventory._hub);
            Player.TryGetItem(serial, out ItemBase itemBase);
            Item = Item.Get(itemBase);
        }

        /// <summary>
        /// Gets the player that the item is being removed from.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the item that is being removed.
        /// </summary>
        public Item Item { get; }
    }
}