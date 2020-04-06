using System;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents the configuration for <see cref="MessageCollector.NextAsync"/>.
    /// </summary>
    public class FilterOptions
    {
        public static FilterOptions Default => new FilterOptions
        {
            ResetTimeoutOnAttempt = false,
            MaxAttempts = null,
            Timeout = TimeSpan.FromSeconds(10)
        };

        /// <summary>
        /// Gets or sets an optional <see cref="TimeSpan"/> that represents the amount of time that is allowed to pass without a new message before automatically closing.
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        
        /// <summary>
        /// Gets or sets an optional 32-bit integer that represents the maximum number of filter attempts before closing.
        /// </summary>
        public int? MaxAttempts { get; set; }

        /// <summary>
        /// Gets or sets a boolean that determines if the specified timeout should reset on any attempt.
        /// </summary>
        public bool ResetTimeoutOnAttempt { get; set; }
    }
}
