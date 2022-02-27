namespace Sharp3dPacking;

public class Item
{
    public string Name { get; }

    public decimal Width { get; }

    public decimal Height { get; }

    public decimal Depth { get; }

    public decimal Weight { get; }

    public decimal Volume { get; }

    public RotationType RotationType { get; internal set; }

    public Position Position { get; internal set; }

    public Item(string name, decimal width, decimal height, decimal depth, decimal weight,
        RotationType rotationType = RotationType.DepthHeightWidth, Position? position = default)
    {
        Name = name;
        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;

        Volume = width * depth * height;

        RotationType = rotationType;
        Position = position ?? Position.StartingPosition;
    }

    public override string ToString() =>
        $"{Name}({Width}x{Height}x{Depth}, weight: {Weight}) pos({Position}) rt({RotationType}:{(int)RotationType}) vol({Volume})";

    /// <summary>
    /// This method will map the dimensions (width, height, and depth) of the item to output variables
    /// based on the orientation (RotationType) of the item.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    internal void RotatedDimensions(out decimal width, out decimal height, out decimal depth)
    {
        switch (RotationType)
        {
            case RotationType.WidthHeightDepth:
                width = Width;
                height = Height;
                depth = Depth;

                return;

            case RotationType.HeightWidthDepth:
                width = Height;
                height = Width;
                depth = Depth;

                return;

            case RotationType.HeightDepthWidth:
                width = Height;
                height = Depth;
                depth = Width;

                return;

            case RotationType.DepthHeightWidth:
                width = Depth;
                height = Height;
                depth = Width;

                return;

            case RotationType.DepthWidthHeight:
                width = Depth;
                height = Width;
                depth = Height;

                return;

            case RotationType.WidthDepthHeight:
                width = Width;
                height = Depth;
                depth = Height;

                return;

            default:
                width = 0;
                height = 0;
                depth = 0;

                return;
        }
    }

    /// <summary>
    /// Returns the position of an item when the item is rotated by
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    internal Position RotatePosition(Axis axis)
    {
        RotatedDimensions(out var width, out var height, out var depth);

        switch (axis)
        {
            case Axis.Width:
                return new Position(Position.X + width, Position.Y, Position.Z);
            case Axis.Height:
                return new Position(Position.X, Position.Y + height, Position.Z);
            case Axis.Depth:
                return new Position(Position.X, Position.Y, Position.Z + depth);
            default:
                return Position.StartingPosition;
        }
    }

    internal decimal PositionAtAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.Width:
                return Position.X;
            case Axis.Height:
                return Position.Y;
            case Axis.Depth:
                return Position.Z;
            default:
                return default;
        }
    }

    internal decimal RotatedPositionAtAxis(Axis axis)
    {
        RotatedDimensions(out var width, out var height, out var depth);

        switch (axis)
        {
            case Axis.Width:
                return width;
                // return Width;
            case Axis.Height:
                return height;
                // return Height;
            case Axis.Depth:
                return depth;
                // return Depth;
            default:
                return default;
        }
    }

    public bool IntersectsWith(Item item) =>
        DoRectanglesIntersect(this, item, Axis.Width, Axis.Height) &&
        DoRectanglesIntersect(this, item, Axis.Height, Axis.Depth) &&
        DoRectanglesIntersect(this, item, Axis.Width, Axis.Depth);

    internal static bool DoRectanglesIntersect(Item item1, Item item2, Axis axis1, Axis axis2)
    {
        var d1a1 = item1.RotatedPositionAtAxis(axis1);
        var cx1 = item1.PositionAtAxis(axis1) + d1a1 / 2;
        var d1a2 = item1.RotatedPositionAtAxis(axis2);
        var cy1 = item1.PositionAtAxis(axis2) + d1a2 / 2;
        
        var d2a1 = item2.RotatedPositionAtAxis(axis1);
        var cx2 = item2.PositionAtAxis(axis1) + d2a1 / 2;
        var d2a2 = item2.RotatedPositionAtAxis(axis2);
        var cy2 = item2.PositionAtAxis(axis2) + d2a2 / 2;

        var ix = Math.Max(cx1, cx2) - Math.Min(cx1, cx2);
        var iy = Math.Max(cy1, cy2) - Math.Min(cy1, cy2);
        
        return ix < (d1a1 + d2a1) / 2 && iy < (d1a2 + d2a2) / 2;
    }
}