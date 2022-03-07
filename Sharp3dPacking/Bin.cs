namespace Sharp3dPacking;

/// <summary>
/// This class describes a container that we'll use to "pack" items into.
/// </summary>
public class Bin
{
    /// <summary>
    /// Arbitrary name of the bin.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Width of the bin.
    /// </summary>
    public decimal Width { get; }
    
    /// <summary>
    /// Height of the bin.
    /// </summary>
    public decimal Height { get; }
    
    /// <summary>
    /// Depth of the bin.
    /// </summary>
    public decimal Depth { get; }
    
    /// <summary>
    /// Maximum weight capacity of the bin.
    /// </summary>
    public decimal MaximumWeightCapacity { get; }
    
    /// <summary>
    /// Computed volume of the bin. This is used for sorting.
    /// </summary>
    public decimal Volume { get; }

    /// <summary>
    /// Computed total weight of all the items contained within the bin.
    /// </summary>
    public decimal TotalWeight => Items.Sum(item => item.Weight);

    /// <summary>
    /// Items contained within the bin.
    /// </summary>
    public List<Item> Items { get; } = new();

    /// <summary>
    /// Items that we attempted to fit into the bin but were unsuccessful. 
    /// </summary>
    public List<Item> UnfittedItems { get; } = new();

    /// <summary>
    /// Basic constructor for the `Bin` class. 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <param name="maximumWeightCapacity"></param>
    public Bin(string name, decimal width, decimal height, decimal depth, decimal maximumWeightCapacity)
    {
        Name = name;
        Width = width;
        Height = height;
        Depth = depth;
        MaximumWeightCapacity = maximumWeightCapacity;

        Volume = width * height * depth;
    }
    
    /// <summary>
    /// Attempts to put an item into a bin...
    /// </summary>
    /// <param name="item">The item that we're attempting to put.</param>
    /// <param name="position">The position of the item that we're attempting to place.</param>
    /// <returns>Boolean value representing whether or not the process was successful.</returns>
    public bool PutItem(Item item, Position position)
    {
        var fit = false; // Items are first assumed to NOT fit.
        
        var initialItemPosition = item.Position; // Store initial item position just in case... 

        item.Position = position; // Assign the candidate item a new position...

        foreach (var rotationType in Enum.GetValues<RotationType>()) // We're going to rotate the item in all six
        {                                                            // directions to see if we can make it fit...
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
                item.Position = initialItemPosition;
            }

            return fit;
        }

        if (!fit)
        {
            item.Position = initialItemPosition;
        }
        
        return fit;
    }

    /// <summary>
    /// [Override]
    /// Outputs a string representation of the `Bin` class in the following format:
    ///
    /// {Name}({Width}x{Depth}x{Height}, max_weight:{MaximumWeightCapacity}) vol({Volume})
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        $"{Name}({Width}x{Depth}x{Height}, max_weight:{MaximumWeightCapacity}) vol({Volume})";
}