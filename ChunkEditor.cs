using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkGenerator))]
public class ChunkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChunkGenerator mapGen = (ChunkGenerator)target;
        if (DrawDefaultInspector())
        {
                mapGen.GenerateMapData();
        }
    }
}
