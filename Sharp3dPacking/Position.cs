namespace Sharp3dPacking;

public class Position
{
    public static readonly Position StartingPosition = new(0, 0, 0);

    public decimal X { get; }
    public decimal Y { get; }
    public decimal Z { get; }

    internal Position(decimal x, decimal y, decimal z)
    {
        X = x;
        Y = z;
        Z = z;
    }
}