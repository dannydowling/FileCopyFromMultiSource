using Microsoft.VisualBasic.FileIO;
public class MultiSourceFileCopy
{
    public void Main(string[] args, string destination)
    {
        GetData(args, destination);
    }

    public async Task GetData(string[] args, string destination) 
    { 
        var sourcePath = new string[args.Length];
        {
            CancellationToken ct = new CancellationToken();
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            };

           await Parallel.ForEachAsync(new[] { sourcePath }, options, async (sourcePath, ct) => 
            {
                Parallel.ForEach(sourcePath, x =>
                {
                    FileSystem.CopyDirectory(x, destination,
                    UIOption.AllDialogs);
                    
                });
                await Task.Yield();
            });
        }
    }
}