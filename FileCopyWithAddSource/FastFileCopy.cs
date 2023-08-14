class FastFileCopy
{
    private const int BufferSize = 4096;

    public async Task CopyFileAsync(string sourcePath, string destinationPath)
    {
        using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.Asynchronous))
        using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, FileOptions.Asynchronous))
        {
            long fileSize = sourceStream.Length;
            byte[] buffer = new byte[BufferSize];

            SemaphoreSlim semaphore = new SemaphoreSlim(Environment.ProcessorCount);
            Task[] copyTasks = new Task[Environment.ProcessorCount];

            for (int i = 0; i < copyTasks.Length; i++)
            {
                copyTasks[i] = CopyChunkAsync(sourceStream, destinationStream, buffer, fileSize, semaphore);
            }

            await Task.WhenAll(copyTasks);
        }
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
