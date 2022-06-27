# FileCopyWithAddSource

The way that this implementation is written is a stupid way.
The problem is that branching has to be context aware of what's already been written by other threads (thread synchronization).
To solve this, I'd like to test the speed of each of the mediums (hard drive, network etc) in an aggregate of the data that is available at all sources.
Then I'd like to divide the aggregate by the speed to give the best possible time for transfer and run each data copy synchronously to the destination.

Thank you for looking.
I feel like the testing and then executing of the system is a lot to program. 
I have this stupid thing here, just because I wrote the dumb thing.
