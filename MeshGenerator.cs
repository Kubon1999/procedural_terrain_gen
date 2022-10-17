using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] noiseMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        int chunkSize = noiseMap.GetLength(0);

        float topLeftX = (chunkSize - 1) / -2f;
        float topLeftZ = (chunkSize - 1) / 2f;

        MeshData meshData = new(chunkSize);
        int vertexIndex = 0;

        for (int height = 0; height < chunkSize; height++)
        {
            for (int width = 0; width < chunkSize; width++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + width, heightCurve.Evaluate(noiseMap[width, height]) * heightMultiplier, topLeftZ - height);
                meshData.uvs[vertexIndex] = new Vector2(width / (float)chunkSize, height / (float)chunkSize);

                if (width < chunkSize - 1 && height < chunkSize - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + chunkSize + 1, vertexIndex + chunkSize);
                    meshData.AddTriangle(vertexIndex + chunkSize + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        return meshData;
    }
}