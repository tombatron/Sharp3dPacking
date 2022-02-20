using System.Net.Mime;

namespace Sharp3dPacking;

public class Item
{
    public string Name { get; }

    public decimal Width { get; }

    public decimal Height { get; }

    public decimal Depth { get; }

    public decimal Weight { get; }

    public decimal Volume { get; }

    public RotationType RotationType { get; }

    public Position Position { get; }

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
        $"{Name}({Width}x{Height}x{Depth}, weight: {Weight}) pos({Position}) rt({RotationType}) vol({Volume})";

    private void RotatedDimensions(out decimal width, out decimal height, out decimal depth)
    {
        switch (RotationType)
        {
            case RotationType.WidthHeightDepth:
                width = Width;
                depth = Height;
                height = Depth;

                return;

            case RotationType.HeightWidthDepth:
                width = Height;
                depth = Width;
                height = Depth;

                return;

            case RotationType.HeightDepthWidth:
                width = Height;
                depth = Depth;
                height = Width;

                return;

            case RotationType.DepthHeightWidth:
                width = Depth;
                depth = Height;
                height = Width;

                return;

            case RotationType.DepthWidthHeight:
                width = Depth;
                depth = Width;
                height = Height;

                return;

            case RotationType.WidthDepthHeight:
                width = Width;
                depth = Depth;
                height = Height;

                return;

            default:
                width = 0;
                depth = 0;
                height = 0;

                return;
        }
    }

    internal Position RotatePosition(Axis axis)
    {
        RotatedDimensions(out var width, out var height, out var depth);
        
        switch (axis)
        {
            case Axis.Depth:
                return new Position(Position.X + width, Position.Y, Position.Z);
            case Axis.Height:
                return new Position(Position.X, Position.Y + height, Position.Z);
            case Axis.Width:
                return new Position(Position.X, Position.Y, Position.Z + depth);
            default:
                return Position.StartingPosition;
        }
    }
}