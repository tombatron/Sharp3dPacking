using System.Diagnostics;

namespace Sharp3dPacking.Example;

public static class Program
{
    public static void Main(string[] args)
    {
        var packer = new Packer();

        packer.AddBin("small-envelope", 11.5m, 6.125m, 0.25m, 10);
        packer.AddBin("large-envelope", 15m, 12m, 0.75m, 15);
        packer.AddBin("small-box", 8.625m, 5.375m, 1.625m, 70);
        packer.AddBin("medium-box", 11m, 8.5m, 5.5m, 70);
        packer.AddBin("medium-2-box", 13.625m, 11.875m, 3.375m, 70);
        packer.AddBin("large-box", 12m, 12m, 5.5m, 70);
        packer.AddBin("large-2-box", 23.6875m, 11.75m, 3.0m, 70);

        packer.AddItem("50g [powder 1]", 3.9370m, 1.9685m, 1.9685m, 1);
        packer.AddItem("50g [powder 2]", 3.9370m, 1.9685m, 1.9685m, 2);
        packer.AddItem("50g [powder 3]", 3.9370m, 1.9685m, 1.9685m, 3);
        packer.AddItem("250g [powder 4]", 7.8740m, 3.9370m, 1.9685m, 4);
        packer.AddItem("250g [powder 5]", 7.8740m, 3.9370m, 1.9685m, 5);
        packer.AddItem("250g [powder 6]", 7.8740m, 3.9370m, 1.9685m, 6);
        packer.AddItem("250g [powder 7]", 7.8740m, 3.9370m, 1.9685m, 7);
        packer.AddItem("250g [powder 8]", 7.8740m, 3.9370m, 1.9685m, 8);
        packer.AddItem("250g [powder 9]", 7.8740m, 3.9370m, 1.9685m, 9);

        var stopwatch = new Stopwatch();

        stopwatch.Start();
        packer.Pack();
        stopwatch.Stop();

        Console.WriteLine($"Packed in: {stopwatch.ElapsedMilliseconds}ms\n\n");

        Console.WriteLine("Bins...\n");

        foreach (var bin in packer.Bins)
        {
            Console.WriteLine($"\n\tContainer Description: {bin}");
            Console.WriteLine("\t=======================================================\n\n");

            Console.WriteLine("\tFitted items...\n");

            foreach (var item in bin.Items)
            {
                Console.WriteLine($"\t\t{item}");
            }

            Console.WriteLine(string.Empty);

            Console.WriteLine("\tUnfitted items...");

            foreach (var item in bin.UnfittedItems)
            {
                Console.WriteLine($"\t\t{item}");
            }

            Console.WriteLine("\t*******************************************************\n\n");
        }
    }
}