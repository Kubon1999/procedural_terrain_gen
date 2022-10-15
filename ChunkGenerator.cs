using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    const int chunkSize = 241;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    public Gradient gradient;
    [Range(0, 6)]
    public int details; //jak bardzo trojkatna
    public float scale;
    public float height;
    public AnimationCurve regionsHeightCurve;
    public int octaves;
    //how much does the small values affect the overall shape around them
    //the bigger the smoother the map will be
    [Range(0, 1)]
    public float persistance;
    // increases frequency of octaves
    // increases the number of small featues in the terrain
    public float lacunarity;
    public int seed;
    public Vector2 offset;


    public void GenerateMap()
    {
        float[,] noiseMap = HelperFunctions.GenerateNoiseMap(chunkSize, chunkSize, seed, scale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[chunkSize * chunkSize];
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                colourMap[y * chunkSize + x] = gradient.Evaluate(noiseMap[x, y]);
            }
        }

        //set mesh for mesh filter and mesh collider
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, height, regionsHeightCurve, details);
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshFilter.sharedMesh.colors = colourMap;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        Texture2D texture = TextureFromColourMap(chunkSize, chunkSize, colourMap);
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    public Texture2D TextureFromColourMap(int width, int height, Color[] colourMap)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    void OnValidate()
    {
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
    }
}
