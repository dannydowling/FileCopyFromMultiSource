# FileCopyWithAddSource

This tests the connection speed of an array of source locations from which to copy to a destination.

The idea is that you might have multiple locations that you back up to and would like to stream from more than one data source.
I've implemented a queue by way of reading 64 bytes to assess each data source.

It also detects how many processors are in the system and uses them.

I have goals of bringing this into a GUI and getting things working faster than the standard Windows filecopy.
Good for Steam Games.
