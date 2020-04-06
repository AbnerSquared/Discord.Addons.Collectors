using System;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents the configuration for <see cref="MessageCollector.HandleAsync"/>.
    /// </summary>
    public class HandlerOptions
    {
        /// <summary>
        /// Gets the default <see cref="HandlerOptions"/>.
        /// </summary>
        public static HandlerOptions Default => new HandlerOptions
        {
            ResetTimeoutOnAttempt = false,
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

        /// <summary>
        /// Gets or sets the <see cref="CollectionHandler"/> that will handle all inbound successful <see cref="FilterResult"/> values.
        /// </summary>
        public CollectionHandler Handler { get; set; }
    }
}
