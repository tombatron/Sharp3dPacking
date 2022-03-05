namespace Sharp3dPacking;

/// <summary>
/// Describes an item that is intended to be placed into a bin. 
/// </summary>
public class Item
{
    /// <summary>
    /// Arbitrary name of an item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Width of an item.
    /// </summary>
    public decimal Width { get; }

    /// <summary>
    /// Height of an item.
    /// </summary>
    public decimal Height { get; }

    /// <summary>
    /// Depth of an item.
    /// </summary>
    public decimal Depth { get; }

    /// <summary>
    /// Weight of an item.
    /// </summary>
    public decimal Weight { get; }

    /// <summary>
    /// Volume of an item. This is computed during object initialization. 
    /// </summary>
    public decimal Volume { get; }

    /// <summary>
    /// How the item is currently rotated.
    /// </summary>
    public RotationType RotationType { get; internal set; }

    /// <summary>
    /// The present position of the item.
    /// </summary>
    public Position Position { get; internal set; }

    /// <summary>
    /// Primary constructor of the `Item` class.
    /// </summary>
    /// <param name="name">Arbitrary name of the item being placed into a bin.</param>
    /// <param name="width">The width of the item.</param>
    /// <param name="height">The height of the item.</param>
    /// <param name="depth">The depth of the item.</param>
    /// <param name="weight">The weight of the item.</param>
    /// <param name="rotationType">How is the item currently rotated. Defaults to "Depth x Width x Height"</param>
    /// <param name="position">The current position of the item. Defaults to 0,0,0</param>
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

    /// <summary>
    /// [Override]
    /// Outputs a string representation of the `Item` class in the following format:
    ///
    /// "{Name}({Width}x{Height}x{Depth}, weight: {Weight}) pos({Position}) rt({RotationType}:{(int)RotationType}) vol({Volume})
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        $"{Name}({Width}x{Height}x{Depth}, weight: {Weight}) pos({Position}) rt({RotationType}:{(int)RotationType}) vol({Volume})";

    /// <summary>
    /// This method will map the dimensions (width, height, and depth) of the item to output variables
    /// based on the orientation (RotationType) of the item.
    ///
    /// The intent behind leveraging the out parameters here is to attempt to reduce allocations by NOT allocating
    /// a tuple, object, or collection. Perhaps a "ref struct" would be better?
    /// </summary>
    /// <param name="width">[out]</param>
    /// <param name="height">[out]</param>
    /// <param name="depth">[out]</param>
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

        return axis switch
        {
            Axis.Width => new Position(Position.X + width, Position.Y, Position.Z),
            Axis.Height => new Position(Position.X, Position.Y + height, Position.Z),
            Axis.Depth => new Position(Position.X, Position.Y, Position.Z + depth),
            _ => Position.StartingPosition
        };
    }

    /// <summary>
    /// Convenience method for getting position values based on a provided axis.
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    internal decimal PositionAtAxis(Axis axis)
    {
        return axis switch
        {
            Axis.Width => Position.X,
            Axis.Height => Position.Y,
            Axis.Depth => Position.Z,
            _ => default
        };
    }

    /// <summary>
    /// Similar to the method `PositionAtAxis` this method will consider the current orientation
    /// of the item before returning the request position at axis.
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    internal decimal RotatedPositionAtAxis(Axis axis)
    {
        RotatedDimensions(out var width, out var height, out var depth);

        switch (axis)
        {
            case Axis.Width:
                return width;
            case Axis.Height:
                return height;
            case Axis.Depth:
                return depth;
            default:
                return default;
        }
    }

    /// <summary>
    /// Determine if this instance of the `Item` class "intersects" with the provided instance
    /// of the `Item` class. 
    /// </summary>
    /// <param name="item">Instance to test against.</param>
    /// <returns></returns>
    public bool IntersectsWith(Item item) =>
        DoRectanglesIntersect(this, item, Axis.Width, Axis.Height) &&
        DoRectanglesIntersect(this, item, Axis.Height, Axis.Depth) &&
        DoRectanglesIntersect(this, item, Axis.Width, Axis.Depth);

    private static bool DoRectanglesIntersect(Item item1, Item item2, Axis axis1, Axis axis2)
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