using Discord.WebSocket;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents the method used to filter a single message based on past collections.
    /// </summary>
    /// <param name="message">The message to filter.</param>
    /// <param name="matches">The collection of previously successful messages.</param>
    /// <param name="index">The position at which this message was filtered (zero-based).</param>
    public delegate bool FilterCollectionDelegate(SocketMessage message, FilterCollection matches, int index);
}
