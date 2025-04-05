using UnityEngine;
using System.Collections.Generic;

public static class ExtendedColors
{
    private static readonly List<Color> ColorList = new()
    {
        new Color32(240,248,255,255),   // AliceBlue
        new Color32(250,235,215,255),   // AntiqueWhite
        new Color32(0,255,255,255),     // Aqua
        new Color32(127,255,212,255),   // Aquamarine
        new Color32(240,255,255,255),   // Azure
        new Color32(245,245,220,255),   // Beige
        new Color32(255,228,196,255),   // Bisque
        new Color32(0,0,0,255),         // Black
        new Color32(255,235,205,255),   // BlanchedAlmond
        new Color32(0,0,255,255),       // Blue
        new Color32(138,43,226,255),    // BlueViolet
        new Color32(165,42,42,255),     // Brown
        new Color32(222,184,135,255),   // Burlywood
        new Color32(95,158,160,255),    // CadetBlue
        new Color32(127,255,0,255),     // Chartreuse
        new Color32(210,105,30,255),    // Chocolate
        new Color32(255,127,80,255),    // Coral
        new Color32(100,149,237,255),   // CornflowerBlue
        new Color32(255,248,220,255),   // Cornsilk
        new Color32(220,20,60,255),     // Crimson
        new Color32(0,255,255,255),     // Cyan
        new Color32(0,0,139,255),       // DarkBlue
        new Color32(0,139,139,255),     // DarkCyan
        new Color32(184,134,11,255),    // DarkGoldenrod
        new Color32(169,169,169,255),   // DarkGray
        new Color32(0,100,0,255),       // DarkGreen
        new Color32(189,183,107,255),   // DarkKhaki
        new Color32(139,0,139,255),     // DarkMagenta
        new Color32(85,107,47,255),     // DarkOliveGreen
        new Color32(255,140,0,255),     // DarkOrange
        new Color32(153,50,204,255),    // DarkOrchid
        new Color32(139,0,0,255),       // DarkRed
        new Color32(233,150,122,255),   // DarkSalmon
        new Color32(143,188,143,255),   // DarkSeaGreen
        new Color32(72,61,139,255),     // DarkSlateBlue
        new Color32(47,79,79,255),      // DarkSlateGray
        new Color32(0,206,209,255),     // DarkTurquoise
        new Color32(148,0,211,255),     // DarkViolet
        new Color32(255,20,147,255),    // DeepPink
        new Color32(0,191,255,255),     // DeepSkyBlue
        new Color32(105,105,105,255),   // DimGray
        new Color32(30,144,255,255),    // DodgerBlue
        new Color32(178,34,34,255),     // FireBrick
        new Color32(255,250,240,255),   // FloralWhite
        new Color32(34,139,34,255),     // ForestGreen
        new Color32(255,0,255,255),     // Fuchsia
        new Color32(220,220,220,255),   // Gainsboro
        new Color32(248,248,255,255),   // GhostWhite
        new Color32(255,215,0,255),     // Gold
        new Color32(218,165,32,255),    // Goldenrod
        new Color32(128,128,128,255),   // Gray
        new Color32(0,128,0,255),       // Green
        new Color32(173,255,47,255),    // GreenYellow
        new Color32(240,255,240,255),   // Honeydew
        new Color32(255,105,180,255),   // HotPink
    };

    public static Color GetColorAt(int index)
    {
        if (index < 0 || index >= ColorList.Count)
        {
            Debug.LogWarning($"[ExtendedColors] 색상 인덱스 {index}는 유효하지 않습니다. 기본 색상으로 대체됩니다.");
            return new Color(0.5f, 0.5f, 0.5f, 1f); // 기본 색상 반환
        }
        return ColorList[index];
    }
}