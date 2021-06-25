# ScrapMetal

### Who am I?
ScrapMetal is a Discord bot stack built in four pieces:
* **SpeakEasy** - the communication library

SpeakEasy provides the underlying websocket networking required to communicate with Discord's Gateway API

* **CandyBar** - the wrapper

CandyBar provides a wrapper around necessary Discord objects & events, and some parts of the Discord HTTP API

* **ScrapMetal** - the robot

ScrapMetal puts the aforementioned projects into use and actually communicates with the Discord APIs

* **RoadRunner** - the launcher

RoadRunner creates a ScrapMetal bot and runs it

### What is my purpose?
The ScrapMetal stack is intended to be a minimalist implementation of a Discord bot, providing only the functionality required to complete a select few tasks

### Why?
There's a lot of Discord wrappers and libraries out there. Most of them are overly complex, attempt to do too much, unstable, etc.
The major aim of the ScrapMetal stack is to be small, maintainable, and usable - it does exactly what it needs to do, and nothing much more. But it should also be pretty straightforward to build onto