using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class ChunkGenerator : MonoBehaviour
{
    public readonly int chunkSize = 255;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    public Gradient gradient;
    public bool Falloff;
    [Range(1, 100)]
    public float scale;
    public float height;
    public AnimationCurve regionsHeightCurve;
    [Range(1, 10)]
    public int octaves;
    //how much does the small values affect the overall shape around them
    //the bigger the smoother the map will be
    [Range(0, 1)]
    public float persistance;
    // increases frequency of octaves
    // increases the number of small featues in the terrain
    [Min(1)]
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    private float[,] falloffMap;


    public void GenerateMapData()
    {
        float[,] noiseMap = HelperFunctions.GenerateNoiseMap(chunkSize, chunkSize, seed, scale, octaves, persistance, lacunarity, offset);

        if (Falloff)
        {
            falloffMap = FalloffGenerator.GenerateFalloffMap(chunkSize);
        }
        Color[] colorMap = new Color[chunkSize * chunkSize];
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                if (Falloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                colorMap[y * chunkSize + x] = gradient.Evaluate(noiseMap[x, y]);
            }
        }


        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, height, regionsHeightCurve);
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshFilter.sharedMesh.colors = colorMap;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        Texture2D texture = TextureFromColorMap(chunkSize, colorMap);
        meshRenderer.sharedMaterial.mainTexture = texture;

    }

    public Texture2D TextureFromColorMap(int chunkSize, Color[] colorMap)
    {
        Texture2D texture = new Texture2D(chunkSize, chunkSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }
}
