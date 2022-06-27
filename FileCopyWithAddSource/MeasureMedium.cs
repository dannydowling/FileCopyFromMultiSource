using System.IO;
using System.Net;
using System.Diagnostics;

public class MeasureMedium 
{
    public long measure(Stream myStream)
    {
        Stopwatch stopwatch = new Stopwatch();
        int offset = 0;
        stopwatch.Reset();
        stopwatch.Start();

        byte[] buffer = new byte[1024]; // 1 KB buffer
        int actualReadBytes = myStream.Read(buffer, offset, buffer.Length);

        // Now we have read 'actualReadBytes' bytes 
        // in 'stopWatch.ElapsedMilliseconds' milliseconds.

        stopwatch.Stop();
        offset += actualReadBytes;
        long speed = (actualReadBytes * 8) / stopwatch.ElapsedMilliseconds; // kbps
        return speed;
        // End of the loop
    }
}
 
        




