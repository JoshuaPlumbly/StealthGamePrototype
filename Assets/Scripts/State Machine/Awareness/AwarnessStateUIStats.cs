using UnityEngine;

[System.Serializable]
public class AwarenessUIStats
{
    public string Header;
    public Color Color;

    public AwarenessUIStats(string header, Color color)
    {
        Header = header;
        Color = color;
    }
}