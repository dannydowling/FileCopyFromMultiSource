# FileCopyWithAddSource

This tests the connection speed of an array of source locations from which to copy to a destination.

The idea is that you might have multiple locations that you back up to and would like to stream from more than one data source.
I've implemented a queue by way of reading 64 bytes to assess each data source.

After sources are ordered, a parallel.foreachasync loop starts copying packets and re-assembling the destination file from the streams.
