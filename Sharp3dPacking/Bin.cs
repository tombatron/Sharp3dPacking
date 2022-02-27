namespace Sharp3dPacking;

public class Bin
{
    public string Name { get; }
    
    public decimal Width { get; }
    
    public decimal Height { get; }
    
    public decimal Depth { get; }
    
    public decimal MaximumWeightCapacity { get; }
    
    public decimal Volume { get; }

    public decimal TotalWeight => Items.Sum(item => item.Weight);

    public List<Item> Items { get; } = new();

    public List<Item> UnfittedItems { get; } = new();

    public Bin(string name, decimal width, decimal height, decimal depth, decimal maximumWeightCapacity)
    {
        Name = name;
        Width = width;
        Height = height;
        Depth = depth;
        MaximumWeightCapacity = maximumWeightCapacity;

        Volume = width * height * depth;
    }
    
    public bool PutItem(Item item, Position position)
    {
        var fit = false;
        var validItemPosition = item.Position;

        item.Position = position;

        foreach (var rotationType in Enum.GetValues<RotationType>())
        {
            item.RotationType = rotationType;

            item.RotatedDimensions(out var width, out var height, out var depth);

            if (Width < position.X + width || Height < position.Y + height || Depth < position.Z + depth)
            {
                continue;
            }

            fit = true;

            foreach (var currentItemInBin in Items)
            {
                // Check to see if any items currently in the bin would intersect with the 
                // item that we're currently looking at.
                if (currentItemInBin.IntersectsWith(item))
                {
                    fit = false;

                    break;
                }
            }

            if (fit)
            {
                if (TotalWeight + item.Weight > MaximumWeightCapacity)
                {
                    fit = false;

                    return fit;                    
                }
                
                Items.Add(item);
            }

            if (!fit)
            {
                item.Position = validItemPosition;
            }

            return fit;
        }

        if (!fit)
        {
            item.Position = validItemPosition;
        }
        
        return fit;
    }

    public override string ToString() =>
        $"{Name}({Width}x{Depth}x{Height}, max_weight:{MaximumWeightCapacity}) vol({Volume})";
}