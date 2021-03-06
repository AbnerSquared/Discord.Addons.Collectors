﻿using System;

namespace Discord.Addons.Collectors
{
    public class MatchOptions
    {
        /// <summary>
        /// Gets the default <see cref="MatchOptions"/>.
        /// </summary>
        public static readonly MatchOptions Default = new MatchOptions
        {
            ResetTimeoutOnAttempt = true,
            Timeout = TimeSpan.FromSeconds(10)
        };

        /// <summary>
        /// The amount of time that is allowed to pass before the <see cref="MessageCollector"/> closes.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Determines if the <see cref="MessageCollector"/> should reset its timeout on any attempt.
        /// </summary>
        public bool ResetTimeoutOnAttempt { get; set; }
    }
}
