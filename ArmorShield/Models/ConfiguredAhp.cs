// -----------------------------------------------------------------------
// <copyright file="ConfiguredAhp.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ArmorShield.Models
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using PlayerStatsSystem;

    /// <summary>
    /// Represents a configured ahp model.
    /// </summary>
    public class ConfiguredAhp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredAhp"/> class.
        /// </summary>
        public ConfiguredAhp()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredAhp"/> class.
        /// </summary>
        /// <param name="startingAmount"><inheritdoc cref="StartingAmount"/></param>
        /// <param name="limit"><inheritdoc cref="Limit"/></param>
        /// <param name="decayRate"><inheritdoc cref="DecayRate"/></param>
        /// <param name="efficacy"><inheritdoc cref="Efficacy"/></param>
        /// <param name="sustain"><inheritdoc cref="Sustain"/></param>
        /// <param name="persistant"><inheritdoc cref="Persistant"/></param>
        public ConfiguredAhp(float startingAmount, float limit = 75f, float decayRate = 1.2f, float efficacy = 0.7f, float sustain = 0f, bool persistant = false)
        {
            StartingAmount = startingAmount;
            Limit = limit;
            DecayRate = decayRate;
            Efficacy = efficacy;
            Sustain = sustain;
            Persistant = persistant;
        }

        /// <summary>
        /// Gets or sets the amount of ahp the player starts with.
        /// </summary>
        [Description("The amount of ahp the player starts with.")]
        public float StartingAmount { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of ahp the player can have.
        /// </summary>
        [Description("The maximum amount of ahp the player can have.")]
        public float Limit { get; set; }

        /// <summary>
        /// Gets or sets the rate at which ahp decays.
        /// </summary>
        [Description("The rate at which ahp decays.")]
        public float DecayRate { get; set; }

        /// <summary>
        /// Gets or sets the efficacy of the ahp.
        /// </summary>
        [Description("The efficacy of the ahp.")]
        public float Efficacy { get; set; }

        /// <summary>
        /// Gets or sets the sustain of the ahp.
        /// </summary>
        [Description("The sustain of the ahp.")]
        public float Sustain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the hume shield will be removed when the players current ahp hits 0.
        /// </summary>
        [Description("Whether the hume shield will be removed when the players current ahp hits 0.")]
        public bool Persistant { get; set; }

        /// <summary>
        /// Applies the ahp process to the provided player.
        /// </summary>
        /// <param name="player">The player to add the process to.</param>
        /// <returns>The created ahp process.</returns>
        public AhpStat.AhpProcess AddTo(Player player) => player.ReferenceHub.playerStats.GetModule<AhpStat>().ServerAddProcess(StartingAmount, Limit, DecayRate, Efficacy, Sustain, Persistant);
    }
}