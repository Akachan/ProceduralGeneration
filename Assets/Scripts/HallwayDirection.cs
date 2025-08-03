using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HallwayDirection {
    Undefined,
    Left,
    Right,
    Top,
    Bottom
};

public static class HallwayDirectionExtensions
{
    //Reemplaza dirección por color
    private static readonly Dictionary<HallwayDirection, Color> DirectionToColorMap =
        new Dictionary<HallwayDirection, Color>()
        {
            { HallwayDirection.Left, Color.yellow },
            { HallwayDirection.Right, Color.magenta },
            { HallwayDirection.Top, Color.cyan },
            { HallwayDirection.Bottom, Color.green },
        };

    public static Color GetColor(this HallwayDirection direction)
    {
        // intenta tomar el color y si no lo encuentra tira gris
        return DirectionToColorMap.TryGetValue(direction, out Color color) ? color : Color.gray;
    }
}