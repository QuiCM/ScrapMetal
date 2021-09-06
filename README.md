# ScrapMetal

ScrapMetal is a Discord bot stack built in four (and a bit) pieces

## Components
#### **SpeakEasy** - the communication library

SpeakEasy provides the underlying websocket networking required to communicate with Discord's Gateway API

#### **CandyBar** - the wrapper

CandyBar provides a minimalist wrapper around necessary Discord objects & events, and some parts of the Discord HTTP API

#### **ScrapMetal** - the robot

ScrapMetal puts the aforementioned projects into use and actually communicates with the Discord APIs

#### **Synapse** - the connector / **Neuron** - the thinker

Synapses are standalone programs that connect Neurons to ScrapMetal, enabling dynamic event processing

You can read more about Synapses in the [Synapse project](Synapse/README.md)

## Build and Run
You can build and run the RoadRunner project to launch a ScrapMetal instance. Note you'll need a Discord Bot auth token
```
dotnet run --project ./ScrapMetal/ScrapMetal.csproj -c Release -- /Discord:ApiToken=YourDiscordApiToken
```

## Why?
The ScrapMetal stack is intended to be a minimalist implementation of a Discord bot. It connects and that's it.
Any functionality is intended to be added in the Synapse layer.

At a high level, ScrapMetal aims to complete the following goals:
* Be simple
* Be stable
* Be extendable

There's a lot of Discord wrappers and libraries out there. Most of them are overly complex, attempt to do too much, are unstable, etc.
ScrapMetal only relies on its own components, and on Microsoft system libraries. This means we have (almost) full control over the entire stack.
Want the websocket to function differently? Adjust SpeakEasy. Want to implement more of the Discord API? Look no further than CandyBar