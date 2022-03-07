using System.Collections.ObjectModel;

namespace Sharp3dPacking;

/// <summary>
/// This class takes a collection of bins and items and attempts to fit the items into the bins.
/// </summary>
public class Packer
{
    private readonly List<Bin> _bins = new();
    private readonly List<Item> _items = new();
    private int _totalItems;

    /// <summary>
    /// A read-only collection of bins that have been associated with this packer. 
    /// </summary>
    public ReadOnlyCollection<Bin> Bins => new(_bins);

    /// <summary>
    /// A read-only collection of the items that have been associated with this packer. 
    /// </summary>
    public ReadOnlyCollection<Item> Items => new(_items);

    /// <summary>
    /// The total number of items assigned to this packer. 
    /// </summary>
    public int TotalItems => _totalItems;

    /// <summary>
    /// The total number of items that could not be fit within a provided container.
    /// </summary>
    public int UnfitItems { get; } = 0;

    /// <summary>
    /// Add a bin to the packer.
    /// </summary>
    /// <param name="bin">An initialized instance of the `Bin` class.</param>
    public void AddBin(Bin bin)
    {
        if (_bins.Count == 0)
        {
            _bins.Add(bin);
            return;
        }

        for (var i = 0; i < _bins.Count; i++)
        {
            if (bin.Volume >= _bins[i].Volume)
            {
                _bins.Insert(i, bin);
                return;
            }
        }

        _bins.Add(bin);
    }

    /// <summary>
    /// Add a bin to the packer by providing initialization values (instead of an instance) of the bin.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <param name="maximumWeightCapacity"></param>
    public void AddBin(string name, decimal width, decimal height, decimal depth, decimal maximumWeightCapacity) =>
        AddBin(new Bin(name, width, height, depth, maximumWeightCapacity));

    /// <summary>
    /// Add multiple bins to the packer by providing a collection of initialized bins.
    /// </summary>
    /// <param name="bins"></param>
    public void AddBins(IEnumerable<Bin> bins) =>
        _bins.AddRange(bins);

    /// <summary>
    /// Add an item to the packer
    /// </summary>
    /// <param name="item">An initialized instance of the `Item` class.</param>
    public void AddItem(Item item)
    {
        _totalItems++;

        if (_items.Count == 0)
        {
            _items.Add(item);
            return;
        }

        for (var i = 0; i < _items.Count; i++)
        {
            if (item.Volume >= _items[i].Volume)
            {
                _items.Insert(i, item);
                return;
            }
        }

        _items.Add(item);
    }

    /// <summary>
    /// Add a item to the packer by providing initialization values (instead of an instance) of the item.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <param name="weight"></param>
    public void AddItem(string name, decimal width, decimal height, decimal depth, decimal weight) =>
        AddItem(new Item(name, width, height, depth, weight));

    /// <summary>
    /// Add multiple items to the packer by providing a collection of initialized items.
    /// </summary>
    /// <param name="items"></param>
    public void AddItems(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    /// <summary>
    /// Start packing!
    /// </summary>
    /// <param name="biggerFirst">Should the algorithm attempt to pack larger items first? (false by default)</param>
    /// <param name="distributeItems">Should the algorithm attempt to distribute items across multiple available bins? (false by default)</param>
    public void Pack(bool biggerFirst = false, bool distributeItems = false)
    {
        var itemStartIndex = biggerFirst ? _items.Count - 1 : 0;
        var itemIncrement = biggerFirst ? -1 : 1;

        var binStartIndex = biggerFirst ? _items.Count - 1 : 0;
        var binIncrement = biggerFirst ? -1 : 1;

        var packed = new bool[_items.Count];
        
        for (var bi = binStartIndex; (bi < _bins.Count && bi >= 0); bi += binIncrement)
        {
            for (var ii = itemStartIndex; (ii < _items.Count && ii >= 0); ii += itemIncrement)
            {
                if (distributeItems && packed[ii])
                {
                    continue;
                }
                
                PackToBin(_bins[bi], _items[ii]);

                packed[ii] = _bins[bi].Items.Contains(_items[ii]);
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