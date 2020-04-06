using Discord.WebSocket;
using System;
using System.Threading.Tasks;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents a handler for collecting inbound messages on Discord.
    /// </summary>
    public class MessageCollector
    {
        private readonly BaseSocketClient _client;

        /// <summary>
        /// Constructs a new <see cref="MessageCollector"/> with the specified <see cref="BaseSocketClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="BaseSocketClient"/> that will be used to read inbound messages.</param>
        public MessageCollector(BaseSocketClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets the total time that elapsed during a handled process (only updated at the end of each handling).
        /// </summary>
        public TimeSpan? ElapsedTime { get; private set; }

        /// <summary>
        /// Tells the <see cref="MessageCollector"/> to begin handling messages.
        /// </summary>
        /// <param name="filter">The filter that will be used to compare messages.</param>
        /// <param name="options">The options that will be used to set up the <see cref="MessageCollector"/>.</param>
        /// <returns></returns>
        public async Task HandleAsync(FilterDelegate filter, HandlerOptions options = null)
        {
            options = options ?? HandlerOptions.Default;

            int index = 0;
            FilterResult match = null;
            var timer = new AsyncTimer(options.Timeout);
            var complete = new TaskCompletionSource<bool>();

            await options.Handler.OnStartAsync();

            async Task HandleAsync(SocketMessage arg)
            {
                bool filterSuccess = filter.Invoke(arg, index);
                match = new FilterResult(arg, index, filterSuccess);

                if (filterSuccess)
                {
                    HandlerResult result = await options.Handler.OnFilterAsync(arg);

                    switch (result)
                    {
                        case HandlerResult.Fail:
                            complete.SetResult(false);
                            break;

                        case HandlerResult.Success:
                            complete.SetResult(true);
                            break;

                        case HandlerResult.Continue:
                        default:
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
            ElapsedTime = timer.ElapsedTime;

            if (timer.Elapsed)
                await options.Handler.OnTimeoutAsync(match?.Message);
        }

        /// <summary>
        /// Tells the <see cref="MessageCollector"/> to collect the next successful message.
        /// </summary>
        /// <param name="filter">The filter that will be used to compare messages.</param>
        /// <param name="options">The options that will be used to set up the <see cref="MessageCollector"/>.</param>
        public async Task<FilterResult> NextAsync(FilterDelegate filter, FilterOptions options = null)
        {
            options = options ?? FilterOptions.Default;

            int attempts = 0;
            FilterResult match = null;
            var timer = new AsyncTimer(options.Timeout);
            var complete = new TaskCompletionSource<bool>();
            
            async Task HandleAsync(SocketMessage arg)
            {
                bool isSuccess = filter.Invoke(arg, attempts);
                match = new FilterResult(arg, attempts, isSuccess);

                if (isSuccess)
                {
                    complete.SetResult(true);
                }
                else if (options.MaxAttempts.HasValue)
                {
                    if (attempts == options.MaxAttempts.Value)
                        complete.SetResult(false);
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
            ElapsedTime = timer.ElapsedTime;

            return match;
        }

        /// <summary>
        /// Tells the <see cref="MessageCollector"/> to begin collecting messages.
        /// </summary>
        /// <param name="filter">The filter that will be used when comparing messages.</param>
        /// <param name="options">The options that will be used to set up the <see cref="MessageCollector"/>.</param>
        public async Task<FilterCollection> CollectAsync(FilterCollectionDelegate filter, CollectionOptions options = null)
        {
            options = options ?? CollectionOptions.Default;

            int index = 0;
            var matches = new FilterCollection();
            var timer = new AsyncTimer(options.Timeout);
            var complete = new TaskCompletionSource<bool>();
            
            async Task HandleAsync(SocketMessage arg)
            {
                bool isSuccess = filter.Invoke(arg, matches, index);

                if (options.IncludeFailedCollections)
                {
                    matches.Add(new FilterResult(arg, index, isSuccess));
                }
                else if (isSuccess)
                {
                    matches.Add(new FilterResult(arg, index, isSuccess));

                    if (options.ResetTimeoutOnCollection)
                        timer.Reset();
                }

                if (matches.Count == options.Capacity)
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

            ElapsedTime = timer.ElapsedTime;
            return matches;
        }
    }
}
