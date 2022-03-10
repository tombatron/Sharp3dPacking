namespace Sharp3dPacking;

/// <summary>
/// Describes the position of an item within a bin.
/// </summary>
public readonly struct Position
{
    /// <summary>
    /// All items start a the origin, this static field is intended as a convenience. 
    /// </summary>
    public static readonly Position StartingPosition = new(0, 0, 0);

    /// <summary>
    /// The X position of the item.
    /// </summary>
    public decimal X { get; }
    
    /// <summary>
    /// The Y position of the item. 
    /// </summary>
    public decimal Y { get; }
    
    /// <summary>
    /// The Z position of the item. 
    /// </summary>
    public decimal Z { get; }

    internal Position(decimal x, decimal y, decimal z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// [Override]
    /// Outputs a string representation of the `Position` struct in the following format:
    ///
    /// x:{X},y:{Y},z:{Z}
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"x:{X},y:{Y},z:{Z}";
}