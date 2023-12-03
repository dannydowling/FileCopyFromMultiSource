using System.Net.NetworkInformation;

public class FileCopier
{
    private const int PacketSize = 1024; // Set your desired packet size in bytes

    public async static void CopyFiles(string[] sourceFilePaths, string destinationFolderPath)
    {
        MeasureMedium measureMedium = new MeasureMedium();
        var orderedSources = measureMedium.orderSources(sourceFilePaths);

        foreach (var source in orderedSources)
        {
            // Check if the source file exists
            if (!File.Exists(source))
            {
                orderedSources.Remove(source);
                continue;
            }
        }

        var options = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 20
        };


        // Iterate through each source file
        await Parallel.ForEachAsync(orderedSources, options, async (sourceFilePath, ct) =>
        {
            // Get the file name from the source file path
            string fileName = Path.GetFileName(sourceFilePath);

            // Calculate the number of packets to copy from
            long fileSizeSource = new FileInfo(sourceFilePath).Length;
            int packetCountSource = (int)Math.Ceiling((double)fileSizeSource / PacketSize);
            int i = packetCountSource;

            // Use Task.Delay to implement a timeout
            Task delayTask = Task.Delay(5000);

            await Task.WhenAny(
                      delayTask,
                      Task.Run(() =>
                      {
                          using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                          {
                              // we're actually reducing the packets left to copy each time the iteration happens.

                              for (i = packetCountSource; i < packetCountSource; i--)
                              {
                                  // Copy the packet to the destination file, cancel if timeout occurs

                                  // Create a packet-sized buffer
                                  byte[] buffer = new byte[PacketSize];

                                  // Read a packet from the source file
                                  int bytesRead = sourceStream.Read(buffer, 0, PacketSize);

                                  // Create the destination file path for the current packet
                                  string destinationFilePath = Path.Combine(destinationFolderPath, $"{fileName}_Part{i + 1}.dat");

                                  // Write the packet to the destination file
                                  using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
                                  {
                                      destinationStream.Write(buffer, 0, bytesRead);
                                  }

                                  Console.WriteLine($"Packet {i + 1} from '{fileName}' copied successfully.");

                              }
                          }
                      }
                      )
                      );
        });
        Console.WriteLine("File copying completed.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Check if at least two arguments are provided
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: FileCopier <destinationFolder> <sourceFile1> [<sourceFile2> ...]");
            return;
        }

        // The first argument is the destination folder
        string destinationFolderPath = args[0];

        // The rest of the arguments are source file paths
        string[] sourceFilePaths = new string[args.Length - 1];
        Array.Copy(args, 1, sourceFilePaths, 0, args.Length - 1);

        // Call the CopyFiles method with the source file paths and destination folder
        FileCopier.CopyFiles(sourceFilePaths, destinationFolderPath);
    }
}