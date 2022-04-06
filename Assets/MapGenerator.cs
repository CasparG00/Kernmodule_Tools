using UnityEngine;

public class MapGenerator
{
    public static Texture2D Generate(Texture2D originalTexture, int mapWidth, int mapHeight)
    {
        var result = originalTexture; 

        result = ScaleTexture(result, mapWidth, mapHeight);
        result = AddNoise(result, 100, 4, 0.5f, 2, Vector2.zero);
        result = StepFilter(result, 0.52f);
        result = CreateOutline(result, Color.red);

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
        
        result.Apply();
        Debug.Log("Rescaling Completed");
        return result;
    }

    private static Texture2D AddNoise(Texture2D original, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[original.width, original.height];
        
        var octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = Random.Range(-100000, 100000) + offset.x;
            float offsetY = Random.Range(-100000, 100000) + offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        }

        if (scale <= 0) 
        {
            scale = 0.0001f;
        }
        
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = original.width * 0.5f;
        float halfHeight = original.height * 0.5f;

        for (int y = 0; y < original.height; y++)
        for (int x = 0; x < original.width; x++)
        {
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for (int i = 0; i < octaves; i++)
            {
                float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
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

        var result = original;

        for (int y = 0; y < original.height; y++)
        for (int x = 0; x < original.width; x++)
        {
            var value = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            result.SetPixel(x, y, Color.Lerp(original.GetPixel(x, y), new Color(value, value, value, 1), 0.5f));
        }

        result.Apply();

        return result;
    }

    private static Texture2D StepFilter(Texture2D original, float edge)
    {
        var result = original;
        
        for (int y = 0; y < original.height; y++)
        for (int x = 0; x < original.width; x++)
        {
            var color = original.GetPixel(x, y);
            
            result.SetPixel(x, y, color.grayscale < edge ? Color.black : Color.white);
        }

        result.Apply();
        
        return result;
    }

    private static Texture2D CreateOutline(Texture2D original, Color color)
    {
        var result = original;
        
        for (int y = 0; y < original.height; y++)
        for (int x = 0; x < original.width; x++)
        {
            if (original.GetPixel(x, y) != Color.black) continue;

            if (original.GetPixel(x + 1, y) == Color.white)
            {
                result.SetPixel(x, y, color);
            }
            
            if (original.GetPixel(x - 1, y) == Color.white)
            {
                result.SetPixel(x, y, color);
            }
            
            if (original.GetPixel(x, y + 1) == Color.white)
            {
                result.SetPixel(x, y, color);
            }
            
            if (original.GetPixel(x, y - 1) == Color.white)
            {
                result.SetPixel(x, y, color);
            }
        }

        result.Apply();

        return result;
    }
}
