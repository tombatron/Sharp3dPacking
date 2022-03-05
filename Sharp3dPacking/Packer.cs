using System.Collections.ObjectModel;

namespace Sharp3dPacking;

public class Packer
{
    private List<Bin> _bins = new();
    private List<Item> _items = new();
    private int _unfitItems = 0;
    private int _totalItems;

    public ReadOnlyCollection<Bin> Bins => new(_bins);

    public ReadOnlyCollection<Item> Items => new(_items);

    public int TotalItems => _totalItems;

    public int UnfitItems => _unfitItems;

    public void AddBin(Bin bin) => _bins.Add(bin);

    public void AddBin(string name, decimal width, decimal height, decimal depth, decimal maximumWeightCapacity) =>
        AddBin(new Bin(name, width, height, depth, maximumWeightCapacity));

    public void AddBins(IEnumerable<Bin> bins) =>
        _bins.AddRange(bins);

    public void AddItem(Item item)
    {
        _totalItems++;

        _items.Add(item);
    }

    public void AddItem(string name, decimal width, decimal height, decimal depth, decimal weight) =>
        AddItem(new Item(name, width, height, depth, weight));

    public void AddItems(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }
    
    public void Pack(bool biggerFirst = false, bool distributeItems = false)
    {
        var sortedBins = (biggerFirst ? _bins.OrderByDescending(x => x.Volume) : _bins.OrderBy(x => x.Volume)).ToList();
        
        var sortedItems = (biggerFirst ? _items.OrderByDescending(x=>x.Volume) : _items.OrderBy(x=>x.Volume)).ToList();

        foreach (var bin in sortedBins)
        {
            foreach (var item in sortedItems)
            {
                PackToBin(bin, item);
            }

            if (distributeItems)
            {
                sortedItems = sortedItems.Where(x => !bin.Items.Contains(x)).ToList();
            }
        }
    }

    private void PackToBin(Bin bin, Item item)
    {
        var fitted = false;

        if (!bin.Items.Any())
        {
            var wasPut = bin.PutItem(item, Position.StartingPosition);

            if (!wasPut)
            {
                bin.UnfittedItems.Add(item);
            }

            return;
        }

        foreach (var axis in Enum.GetValues<Axis>())
        {
            var itemsInBin = bin.Items;

            foreach (var itemInBin in itemsInBin)
            {
                var pivot = itemInBin.RotatePosition(axis);

                if (bin.PutItem(item, pivot))
                {
                    fitted = true;

                    break;
                }
            }

            if (fitted)
            {
                break;
            }
        }

        if (!fitted)
        {
            bin.UnfittedItems.Add(item);
        }
    }
}