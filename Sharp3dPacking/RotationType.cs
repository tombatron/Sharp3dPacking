namespace Sharp3dPacking;

/// <summary>
/// Enumeration describing how an item is positioned within a bin. 
/// </summary>
public enum RotationType
{
    /// <summary>
    /// Width x Height X Depth
    /// </summary>
    WidthHeightDepth,
    
    /// <summary>
    /// Height x Width x Depth
    /// </summary>
    HeightWidthDepth,
    
    /// <summary>
    /// Height x Depth x Width
    /// </summary>
    HeightDepthWidth,
    
    /// <summary>
    /// Depth x Height x Width
    /// </summary>
    DepthHeightWidth,
    
    /// <summary>
    /// Depth x Width x Height
    /// </summary>
    DepthWidthHeight,
    
    /// <summary>
    /// Width x Depth x Height
    /// </summary>
    WidthDepthHeight
}

internal static class RotationTypes
{
    internal static readonly RotationType[] Instances = {
        RotationType.WidthHeightDepth,
        RotationType.HeightWidthDepth,
        RotationType.HeightDepthWidth,
        RotationType.DepthHeightWidth,
        RotationType.DepthWidthHeight,
        RotationType.WidthDepthHeight
    };
}