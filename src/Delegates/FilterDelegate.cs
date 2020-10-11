using Discord.WebSocket;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents a generic message filter.
    /// </summary>
    /// <param name="message">The message that was read.</param>
    /// <param name="index">The current counter of messages handled.</param>
    public delegate bool FilterDelegate(SocketMessage message, int index);
}
