using Discord.WebSocket;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents the method used to filter a single message.
    /// </summary>
    /// <param name="message">The message to filter.</param>
    /// <param name="index">The position at which this message was filtered (zero-based).</param>
    public delegate bool FilterDelegate(SocketMessage message, int index);
}
