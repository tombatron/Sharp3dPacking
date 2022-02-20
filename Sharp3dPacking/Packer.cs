namespace Sharp3dPacking;

public class Packer
{
    private List<Bin> _bins = new();
    private List<Item> _items = new();
    private int _unfitItems = 0;
    private int _totalItems = 0;

    public void AddBin(Bin bin) => _bins.Add(bin);

    public void AddItem(Item item)
    {
        _totalItems++;
        
        _items.Add(item);
    }

    public void Pack(bool biggerFirst = false, bool distributeItems = false)
    {
        // TODO: Sort bins... biggerFirst will essentially be descending...
        var sortedBins = _bins;
        // TODO: Sort items...
        var sortedItems = _items;

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

        if (!_bins.Any())
        {
            var wasPut = bin.PutItem(item, Position.StartingPosition);

            if (!wasPut)
            {
                bin.UnfittedItems.Add(item);
            }
            
            return;
        }

        for (var i = 0; i < 3; i++)
        {
            var axis = (Axis) i;
            var itemsInBin = bin.Items;

            foreach (var itemInBin in itemsInBin)
            {
                var pivot = itemInBin.RotatePosition(axis);

                if (bin.PutItem(itemInBin, pivot))
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