using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    private Planet _planet;
    private Texture2D texture;
    private const int TextureResolution = 100;
    private static readonly int ElevationMinMax = Shader.PropertyToID("_elevationMinMax");
    private static readonly int TexturePropName = Shader.PropertyToID("_texture");

    public void UpdateSettings(Planet planet)
    {
        _planet = planet;
        if (texture == null)
            texture = new Texture2D(TextureResolution, 1);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _planet.ColorSettings.planetMaterial.SetVector(ElevationMinMax,
            new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        var colors = new Color[TextureResolution];
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = _planet.ColorSettings.gradient.Evaluate(i / (TextureResolution - 1f));
        }

        texture.SetPixels(colors);
        texture.Apply();
        _planet.ColorSettings.planetMaterial.SetTexture(TexturePropName, texture);
    }
}