// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ArmorShield
{
    using System.Collections.Generic;
    using System.Linq;
    using ArmorShield.Models;
    using Exiled.API.Extensions;
    using Exiled.API.Interfaces;

    /// <inheritdoc />
    public class Config : IConfig
    {
        private Dictionary<ItemType, ConfiguredAhp> armorShields = new()
        {
            { ItemType.ArmorLight, new ConfiguredAhp(30f, 30f, -1f, 0.7f, 10f, true) },
            { ItemType.ArmorCombat, new ConfiguredAhp(40f, 40f, -1.5f, 0.7f, 12.5f, true) },
            { ItemType.ArmorHeavy, new ConfiguredAhp(60f, 60f, -2f, 0.7f, 15f, true) },
        };

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the armor types and their respective shields.
        /// </summary>
        public Dictionary<ItemType, ConfiguredAhp> ArmorShields
        {
            get => armorShields;
            set
            {
                foreach (KeyValuePair<ItemType, ConfiguredAhp> kvp in value.ToList())
                {
                    if (!kvp.Key.IsArmor())
                        value.Remove(kvp.Key);
                }

                armorShields = value;
            }
        }
    }
}