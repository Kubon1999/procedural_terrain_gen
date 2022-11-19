using UnityEngine;

public class HelperFunctions : MonoBehaviour
{

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octavesNr, float persistance, float lacunarity, Vector2 mapOffset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        var octaveOffsets = GetOctaveOffsets(mapOffset, octavesNr, seed);
        float halfWidth = mapWidth / 2.0f;
        float halfHeight = mapHeight / 2.0f;

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for (int height = 0; height < mapHeight; height++)
        {
            for (int width = 0; width < mapWidth; width++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //octave loop
                for (int i = 0; i < octavesNr; i++)
                {
                    float sampleX = (width - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (height - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                else if (noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }
                noiseMap[width, height] = noiseHeight;
            }
        }
        return NormalizeNoiseMap(noiseMap, mapHeight, mapWidth, minHeight, maxHeight);
    }

    private static Vector2[] GetOctaveOffsets(Vector2 offset, int octaves, int seed)
    {
        Vector2[] octaveOffsets = new Vector2[octaves];
        System.Random random = new(seed);
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }

    private static float[,] NormalizeNoiseMap(float[,] noiseMap, int mapHeight, int mapWidth, float minHeight, float maxHeight)
    {
        for (int height = 0; height < mapHeight; height++)
        {
            for (int width = 0; width < mapWidth; width++)
            {
                noiseMap[width, height] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[width, height]);
            }
        }
        return noiseMap;
    }
}