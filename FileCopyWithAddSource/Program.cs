public class MultiSourceFileCopy
{

    private const int BufferSize = 4096;
    private List<string> sourceOrdering;
    public void Main(string[] args, string destination)
    {
        CopyFileAsync(args, destination);
    }

    public async Task CopyFileAsync(string[] args, string destination)
    {
        var sourcePath = new string[args.Length];
        {
            CancellationToken ct = new CancellationToken();

            MeasureMedium measureMedium = new MeasureMedium();

            var orderedSources = measureMedium.orderSources(args);

            for (int i = 0; i < orderedSources.Count;)
            {
                //here's the magic. Take the top choice and use that source to copy to the destination
                using (FileStream sourceStream = new FileStream(orderedSources.ElementAt(i), FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.Asynchronous))
                using (FileStream destinationStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, FileOptions.Asynchronous))
                {
                    long fileSize = sourceStream.Length;
                    byte[] buffer = new byte[BufferSize];


                    SemaphoreSlim semaphore = new SemaphoreSlim(Environment.ProcessorCount);
                    Task[] copyTasks = new Task[Environment.ProcessorCount];
                    Comparator comparator = new StreamLengthComparator();

                    for (int j = 0; comparator.Equals(destinationStream.Length(sourceStream.Length); j++)
                    {
                        copyTasks[j] = CopyChunkAsync(sourceStream, destinationStream, buffer, fileSize, semaphore);
                        sourceOrdering = measureMedium.orderSources(args);
                    }

                    await Task.WhenAll(copyTasks);
                }
            }
        }
    }


    public class StreamLengthComparator : Comparator
    {
    }

    private async Task CopyChunkAsync(FileStream sourceStream, FileStream destinationStream, byte[] buffer, long fileSize, SemaphoreSlim semaphore)
    {
        int bytesRead;
        while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await semaphore.WaitAsync();
            try
            {
                await destinationStream.WriteAsync(buffer, 0, bytesRead);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
