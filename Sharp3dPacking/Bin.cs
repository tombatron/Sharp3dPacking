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
    
    // TODO: Implement `put_item`: https://github.com/enzoruiz/3dbinpacking/blob/e8c6e6ecfd79574eeae126fad498351790fbd5bf/py3dbp/main.py#L93
    public bool PutItem(Item item, Position position)
    {
        return false;
    }

    public override string ToString() =>
        $"{Name}({Width}x{Depth}x{Height}, max_weight:{MaximumWeightCapacity}) vol({Volume})";
}