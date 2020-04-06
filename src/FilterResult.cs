using Discord.WebSocket;
using System;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents a result of a filtered <see cref="SocketMessage"/> from a <see cref="MessageCollector"/>.
    /// </summary>
    public class FilterResult
    {
        internal FilterResult(SocketMessage message, int index, bool isSuccess)
        {
            Message = message;
            Index = index;
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// Gets a 32-bit integer that represents the index of the <see cref="FilterResult"/> (zero-based).
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the message that was received from the <see cref="MessageCollector"/>.
        /// </summary>
        public SocketMessage Message { get; }

        /// <summary>
        /// Gets a boolean that defines if the <see cref="FilterResult"/> was a success.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Converts the <see cref="FilterResult"/> into the specified enclosing <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TValue">The enclosing <see cref="Type"/> that the <see cref="FilterResult"/> will convert to.</typeparam>
        /// <param name="converter">The method used to convert the <see cref="FilterResult"/>.</param>
        public TValue Convert<TValue>(Func<FilterResult, TValue> converter)
        {
            return converter.Invoke(this);
        }
    }
}
