using Discord.WebSocket;
using System;
using System.Threading.Tasks;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents a handler for collecting inbound messages on <see cref="Discord"/>.
    /// </summary>
    public class MessageCollector
    {
        private readonly BaseSocketClient _client;

        /// <summary>
        /// Initializes a new <see cref="MessageCollector"/> with the specified <see cref="BaseSocketClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="BaseSocketClient"/> that will be referenced to read inbound messages.</param>
        public MessageCollector(BaseSocketClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Starts an asynchronous <see cref="MessageSession"/> for this <see cref="MessageCollector"/>.
        /// </summary>
        /// <param name="session">The <see cref="MessageSession"/> that will be used for this <see cref="MessageCollector"/>.</param>
        /// <param name="filter">The filter that will be used to compare messages.</param>
        /// <param name="options">The options that will be used to set up the <see cref="MessageCollector"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> that represents the amount of time that has passed for this method.</returns>
        public async Task<TimeSpan> RunSessionAsync(MessageSession session, FilterDelegate filter, MatchOptions options = null)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session), "The specified MessageSession cannot be null");

            options ??= MatchOptions.Default;
            SocketMessage previous = null;
            var timer = new AsyncTimer(options.Timeout);
            var complete = new TaskCompletionSource<bool>();

            await session.OnStartAsync();

            int index = 0;
            async Task HandleAsync(SocketMessage arg)
            {
                bool filterSuccess = filter.Invoke(arg, index);
                previous = arg;

                if (filterSuccess)
                {
                    SessionResult result = await session.OnMessageReceivedAsync(arg);

                    switch (result)
                    {
                        case SessionResult.Success:
                            complete.SetResult(true);
                            break;

                        case SessionResult.Fail:
                            complete.SetResult(false);
                            break;
                    }
                }

                if (options.ResetTimeoutOnAttempt)
                    timer.Reset();

                index++;
            }

            _client.MessageReceived += HandleAsync;

            if (options.Timeout.HasValue)
                timer.Start();

            await Task.WhenAny(timer.CompletionSource.Task, complete.Task);

            _client.MessageReceived -= HandleAsync;

            if (timer.Elapsed)
                await session.OnTimeoutAsync(previous);

            return timer.ElapsedTime;
        }

        /// <summary>
        /// Tells the <see cref="MessageCollector"/> to asynchronously attempt to match a single message.
        /// </summary>
        /// <param name="filter">The raw filter that will be used to compare messages.</param>
        /// <param name="options">The options that will be used to set up the <see cref="MessageCollector"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> that represents the amount of time that has passed for this method.</returns>
        public async Task<MessageMatch> MatchAsync(FilterDelegate filter, MatchOptions options = null)
        {
            options ??= MatchOptions.Default;
            MessageMatch match = null;

            var timer = new AsyncTimer(options.Timeout);
            var complete = new TaskCompletionSource<bool>();

            int attempts = 0;
            async Task HandleAsync(SocketMessage arg)
            {
                bool isSuccess = filter.Invoke(arg, attempts);
                match = new MessageMatch(arg, attempts, isSuccess, timer.ElapsedTime);

                if (isSuccess)
                {
                    complete.SetResult(true);
                }

                if (options.ResetTimeoutOnAttempt)
                    timer.Reset();

                attempts++;
            }

            _client.MessageReceived += HandleAsync;

            if (options.Timeout.HasValue)
                timer.Start();

            await Task.WhenAny(timer.CompletionSource.Task, complete.Task);

            _client.MessageReceived -= HandleAsync;
            return match;
        }

        /// <summary>
        /// Tells the <see cref="MessageCollector"/> to begin collecting messages asynchronously.
        /// </summary>
        /// <param name="filter">The raw filter that will be used when comparing messages.</param>
        /// <param name="options">The options that will be used to set up the <see cref="MessageCollector"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> that represents the amount of time that has passed for this method.</returns>
        public async Task<MessageMatchCollection> CollectAsync(FilterCollectionDelegate filter, CollectionOptions options = null)
        {
            options ??= CollectionOptions.Default;
            var matches = new MessageMatchCollection();
            var timer = new AsyncTimer(options.Timeout);
            var complete = new TaskCompletionSource<bool>();

            int index = 0;
            async Task HandleAsync(SocketMessage arg)
            {
                bool isSuccess = filter.Invoke(arg, matches, index);

                if (isSuccess || options.IncludeFailedMatches)
                    matches.Add(new MessageMatch(arg, index, isSuccess, timer.ElapsedTime));

                if (isSuccess && options.ResetTimeoutOnMatch)
                    timer.Reset();

                if (options.Capacity.HasValue && matches.Count == options.Capacity.Value)
                {
                    timer.Stop();
                    complete.SetResult(true);
                }

                index++;
            }

            _client.MessageReceived += HandleAsync;

            if (options.Timeout.HasValue)
                timer.Start();

            await Task.WhenAny(timer.CompletionSource.Task, complete.Task);

            _client.MessageReceived -= HandleAsync;
            return matches;
        }
    }
}
