<img src="./marketing/Icon.png" width="64" height="64" /><br/>
# Discord.Addons.Collectors
> See also: [**Discord.Addons.Linking**](https://github.com/AbnerSquared/Discord.Addons.Linking)<br/>

[![NuGet](https://img.shields.io/nuget/vpre/Discord.Addons.Collectors.svg?maxAge=2592000?style=plastic)](https://www.nuget.org/packages/Discord.Addons.Collectors)<br/>
An extension for Discord.Net that provides classes to filter and collect messages.

## Usage
To initialize a new `MessageCollector`, you require an instance of a `Discord.Websocket.BaseSocketClient`. Any inheriting types of this class also works.
```cs
private readonly BaseSocketClient _client;
var collector = new MessageCollector(_client);
```

It might also help to set up a default filter method that can be used as a reference for any future matches you intend to use. All of the required parameters can be omitted if you wish to use a custom filter of your own, as long as the contents of your filter are stored in this method. Here is an example of a `MessageCollector` in applied use:
```cs
public class DummyModule : ModuleBase<SocketCommandContext>
{
	private readonly MessageCollector _collector;

	// The MessageCollector class can be specified as a singleton for dependency injection instead
	// This is a 'hacky' way to get around that for this example
	public DummyModule(DiscordSocketClient client)
	{
		_collector = new MessageCollector(client);
	}

	private bool Judge(SocketMessage message, int index)
	{
		return Context.User.Id == message.Author.Id  && Context.Channel.Id == message.Channel.Id && message.Content == "4";		
	}

	[Command("demo")]
	public async Task DummyCommandAsync()
	{
		var options = new MatchOptions
		{
			Timeout = TimeSpan.FromSeconds(10),
			ResetTimeoutOnAttempt = false
		};

		IUserMessage message = await Context.Channel.SendMessageAsync("What is 2+2?");

		MessageMatch match = await _collector.MatchAsync(Judge, options);

		// If the match is null, this means that the collector failed to receive any matches within the specified time limit
		if (match == null)
		{
			await message.ModifyAsync(m => m.Content = "No response was given. The answer was 4.");
		}
		else
		{
			await message.ModifyAsync(m => m.Content = "Correct! You get a gold star in my book.");
		}
	}
}
```

There are three methods that can be used in a `MessageCollector`:

- `MessageCollector.MatchAsync(FilterDelegate, MatchOptions)`: Attempt to find a single message that matches the specified filter
- `MessageCollector.CollectAsync(FilterCollectionDelegate, CollectionOptions)`: Attempt to collect all messages that match the specified filter
- `MessageCollector.RunSessionAsync(MessageSession, FilterDelegate, SessionOptions)`: Starts the specified message session

Message sessions are a useful way to handle messages in an advanced manner. These can be built in any way to give you freedom on how a session should be handled.
Here is an example of a simple message session:
```cs
public class DummySession : MessageSession
{
	public DummySession(SocketCommandContext context)
	{
		Context = context;
		Attempts = 0;
	}

	private SocketCommandContext Context { get; }
	private int Attempts { get; set; }
	private IUserMessage MessageReference { get; set; }


	public override async Task OnStartAsync()
	{
		MessageReference = await Context.Channel.SendMessageAsync("What is 2+2?");
	}

	public override async Task<SessionResult> OnMessageReceivedAsync(SocketMessage message)
	{
		if (message.Content == "4")
		{
			await MessageReference.ModifyAsync(delegate (MessageProperties x)
			{
				x.Content = "Correct!";
			});

			return SessionResult.Success;
		}

		Attempts++;

		if (Attempts >= 3)
		{
			await MessageReference.ModifyAsync(delegate (MessageProperties x)
			{
				x.Content = "You ran out of attempts. The answer was 4.";
			});
		}
	}

	public override async Task OnTimeoutAsync()
	{
		await MessageReference.ModifyAsync(delegate (MessageProperties x)
		{
			x.Content = "You ran out of time. The answer was 4.";
		});
	}
}
```

Using this custom session, that example command from before can be tweaked:
```cs
public class DummyModule : ModuleBase<SocketCommandContext>
{
	private readonly MessageCollector _collector;

	public DummyModule(DiscordSocketClient client)
	{
		_collector = new MessageCollector(client);
	}

	private bool Judge(SocketMessage message, int index)
	{
		return Context.User.Id == message.Author.Id && Context.Channel.Id == message.Channel.Id;		
	}

	[Command("demo")]
	public async Task DummyCommandAsync()
	{
		var options = new MatchOptions
		{
			Timeout = TimeSpan.FromSeconds(5),
			ResetTimeoutOnAttempt = true
		};

		var session = new DummySession(Context);
		// The result of this method returns a TimeSpan containing the time that was elapsed for this session
		// This can easily be omitted if this is undesired
		TimeSpan elapsedTime = await _collector.RunSessionAsync(session, Judge, options);
	}
}
```
