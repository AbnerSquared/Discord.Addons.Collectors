﻿namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Defines what a <see cref="MessageSession"/> proceeds with once a <see cref="MessageMatch"/> is handled.
    /// </summary>
    public enum SessionResult
    {
        /// <summary>
        /// Closes the <see cref="MessageCollector"/> and marks the proceeding action as a success.
        /// </summary>
        Success = 1,

        /// <summary>
        /// Closes the <see cref="MessageCollector"/> and marks the proceeding action as a failure.
        /// </summary>
        Fail = 2,

        /// <summary>
        /// Allows the <see cref="MessageCollector"/> to continue processing messages.
        /// </summary>
        Continue = 3
    }
}
