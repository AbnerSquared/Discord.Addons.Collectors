using System;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents the configuration for <see cref="MessageCollector.CollectAsync"/>.
    /// </summary>
    public class CollectionOptions
    {
        /// <summary>
        /// Gets the default <see cref="CollectionOptions"/>.
        /// </summary>
        public static CollectionOptions Default = new CollectionOptions
        {
            Capacity = null,
            IncludeFailedCollections = false,
            Timeout = TimeSpan.FromSeconds(30),
            ResetTimeoutOnCollection = false
        };
        
        /// <summary>
        /// Gets or sets an optional <see cref="TimeSpan"/> that represents the amount of time that is allowed to pass without a new message before automatically closing.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets or sets a boolean that determines if the specified timeout should reset on a collection.
        /// </summary>
        public bool ResetTimeoutOnCollection { get; set; } = false;

        /// <summary>
        /// Gets or sets an optional 32-bit integer that determines the maximum size of collected messages before automatically closing.
        /// </summary>
        public int? Capacity { get; set; } = null;

        /// <summary>
        /// Gets or sets a boolean that determines if failed filter attempts are also collected.
        /// </summary>
        public bool IncludeFailedCollections { get; set; } = false;
    }
}
