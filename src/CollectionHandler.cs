using Discord.WebSocket;
using System.Threading.Tasks;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents an abstract handler that applies to each successful <see cref="FilterResult"/>.
    /// </summary>
    public abstract class CollectionHandler
    {
        /// <summary>
        /// Represents the method that is invoked whenever a <see cref="MessageCollector"/> starts filtering messages.
        /// </summary>
        public virtual async Task OnStartAsync() { }

        /// <summary>
        /// Represents the method that is invoked whenever a <see cref="MessageCollector"/> receives a successful <see cref="FilterResult"/>.
        /// </summary>
        public abstract Task<HandlerResult> OnFilterAsync(SocketMessage message);

        /// <summary>
        /// Represents the method that is invoked whenever a <see cref="MessageCollector"/> runs out of time.
        /// </summary>
        public abstract Task OnTimeoutAsync(SocketMessage message);

        /// <summary>
        /// Represents the method that is invoked whenever a <see cref="MessageCollector"/> is cancelled.
        /// </summary>
        public virtual async Task OnCancelAsync() { }
    }
}
