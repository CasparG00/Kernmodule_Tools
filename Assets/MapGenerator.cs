using System.Linq;
using UnityEngine;

public static class MapGenerator
{
    public static Texture2D Generate(Texture2D originalTexture, int mapWidth, int mapHeight)
    {
        var result = originalTexture; 

        result = ScaleTexture(result, mapWidth, mapHeight);
        result = AddNoise(result, 100, 4, 0.5f, 2, Vector2.zero);
        result = StepFilter(result, 0.52f);
        result = CreateOutline(result, Color.red, 3);
        
        result.Apply();

        return result;
    }
    
    private static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        var result = new Texture2D(targetWidth, targetHeight, source.format, false);
        for (var i = 0; i < result.height; ++i)
        for (var j = 0; j < result.width; ++j)
        {
            var newColor = source.GetPixelBilinear(j / (float)result.width, i / (float)result.height);
            result.SetPixel(j, i, newColor);
        }
        
        return result;
    }

    private static Texture2D AddNoise(Texture2D original, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        var noiseMap = new float[original.width, original.height];
        
        var octaveOffsets = new Vector2[octaves];
        for (var i = 0; i < octaves; i++)
        {
            var offsetX = Random.Range(-100000, 100000) + offset.x;
            var offsetY = Random.Range(-100000, 100000) + offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        }

        if (scale <= 0) 
        {
            scale = 0.0001f;
        }
        
        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        var halfWidth = original.width * 0.5f;
        var halfHeight = original.height * 0.5f;

        for (var y = 0; y < original.height; y++)
        for (var x = 0; x < original.width; x++)
        {
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for (var i = 0; i < octaves; i++)
            {
                var sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                var sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseHeight += perlinValue * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            if (noiseHeight > maxNoiseHeight)
            {
                maxNoiseHeight = noiseHeight;
            }
            else if (noiseHeight < minNoiseHeight)
            {
                minNoiseHeight = noiseHeight;
            }

            noiseMap[x, y] = noiseHeight;
        }

        for (var y = 0; y < original.height; y++)
        for (var x = 0; x < original.width; x++)
        {
            var value = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            original.SetPixel(x, y, Color.Lerp(original.GetPixel(x, y), new Color(value, value, value, 1), 0.5f));
        }
        
        return original;
    }

    private static Texture2D StepFilter(Texture2D original, float edge)
    {
        for (var y = 0; y < original.height; y++)
        for (var x = 0; x < original.width; x++)
        {
            var color = original.GetPixel(x, y);
            
            original.SetPixel(x, y, color.grayscale < edge ? Color.black : Color.white);
        }
        
        return original;
    }

    private static Texture2D CreateOutline(Texture2D original, Color color, int thickness)
    {
        var result = new Texture2D(original.width, original.height)
        {
            filterMode = FilterMode.Point
        };
        
        result.SetPixels(Enumerable.Repeat(Color.white, result.width * result.height).ToArray());
        
        for (var y = 0; y < original.height; y++)
        for (var x = 0; x < original.width; x++)
        {
            if (original.GetPixel(x, y) != Color.black) continue;

            var extents = Mathf.RoundToInt(thickness);
            
            for (var neighbourX = -1 - extents; neighbourX < 2 + extents; neighbourX++)
            for (var neighbourY = -1 - extents; neighbourY < 2 + extents; neighbourY++)
            {
                if (Mathf.Abs(neighbourX) == Mathf.Abs(neighbourY)) continue;

                var checkX = x + neighbourX;
                var checkY = y + neighbourY;
                
                if (original.GetPixel(checkX, checkY) == Color.black) continue;

                if (original.GetPixel(checkX, checkY) == Color.white)
                {
                    result.SetPixel(x, y, color);
                }
            }
        }
        
        return result;
    }
}
