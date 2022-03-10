using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;

namespace Sharp3dPacking.Benchmarks;

[MemoryDiagnoser]
public class ExampleProjectBenchmark
{
    private IEnumerable<Item>? _items;
    private IEnumerable<Bin>? _bins;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _items = new List<Item>()
        {
            new("50g [powder 1]", 3.9370m, 1.9685m, 1.9685m, 1),
            new("50g [powder 2]", 3.9370m, 1.9685m, 1.9685m, 2),
            new("50g [powder 3]", 3.9370m, 1.9685m, 1.9685m, 3),
            new("250g [powder 4]", 7.8740m, 3.9370m, 1.9685m, 4),
            new("250g [powder 5]", 7.8740m, 3.9370m, 1.9685m, 5),
            new("25g [powder 6]", 7.8740m, 3.9370m, 1.9685m, 6),
            new("250g [powder 7]", 7.8740m, 3.9370m, 1.9685m, 7),
            new("250g [powder 8]", 7.8740m, 3.9370m, 1.9685m, 8),
            new("250g [powder 9]", 7.8740m, 3.9370m, 1.9685m, 9)
        };
        
        _bins = new List<Bin>()
        {
            new("small-envelope", 11.5m, 6.125m, 0.25m, 10),
            new("large-envelope", 15m, 12m, 0.75m, 15),
            new("small-box", 8.625m, 5.375m, 1.625m, 70),
            new("medium-box", 11m, 8.5m, 5.5m, 70),
            new("medium-2-box", 13.625m, 11.875m, 3.375m, 70),
            new("large-box", 12m, 12m, 5.5m, 70),
            new("large-2-box", 23.6875m, 11.75m, 3.0m, 70)
        };
    }

    [Benchmark]
    public (ReadOnlyCollection<Bin> Bins, ReadOnlyCollection<Item> Items, int TotalItems, int UnfitItems) ExampleCase()
    {
        if (_bins is null || _items is null)
        {
            throw new Exception("Gonna need some bins and items.");
        }

        var packer = new Packer();

        packer.AddBins(_bins);
        packer.AddItems(_items);

        packer.Pack();

        return (packer.Bins, packer.Items, packer.TotalItems, packer.UnfitItems);
    }
}