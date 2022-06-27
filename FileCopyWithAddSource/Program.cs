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

            var paths = new[] { sourcePath };
            await Parallel.ForEachAsync(paths, options, async (sourcePath, ct) =>
             {
                 Parallel.ForEach(sourcePath, x =>
                 {
                     //create the file stream to each of the endpoints
                    FileStream myStream = new FileStream(x, FileMode.Create);

                     MeasureMedium measureMedium = new MeasureMedium();
                     //create an array to hold the speed that each path equates to
                     long[] speed = new long[paths.Length];

                     for (int i = 0; i < paths.Length; i++)
                     {
                         speed[i] = measureMedium.measure(myStream);

                         var totalSpeed = new long();
                         for (long j = 0; j < speed.Length; j++)
                         {
                             j += totalSpeed;
                         }

                         var percentageOfTotal = speed[i] / totalSpeed;

                         List<string> files = new List<string>();
                         foreach (var file in FileSystem.GetFiles(sourcePath[i]))
                         {
                             files.Add(file);
                         }

                         //now files has a string list of all files. We have a number that correlates to the fraction of the total speed.
                         List<string> medium_list = new List<string>();

                         for (int k = 0; k < percentageOfTotal; k++)
                         {
                             medium_list.Add(files[k]);
                         files.Remove(files[k]);
                         }

                         foreach (var item in medium_list)
                         {
                             // copy the files
                             FileSystem.CopyDirectory(item, destination, UIOption.AllDialogs);
                         }
                     }
                 });
                 await Task.Yield();
             });
        }
    }
}