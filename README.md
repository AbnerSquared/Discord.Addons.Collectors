# Discord.Addons.Collectors
An extension for Discord.Net that provides classes to filter and collect messages.

# Purpose
This extension was made to simplify the process of dynamic commands. This extension provides a class that can handle inbound messages in a much easier way in three styles:

### Collect
This style allows you to collect messages with a specified filter. If the filter is successful, the message is collected.

### Next
This style allows you to get the next successfully filtered message.

### Handle
This style gives you complete control over what happens across each message. Using a `CollectionHandler` class, you can determine what happens on each filtered message.
