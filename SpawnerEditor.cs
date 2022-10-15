using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        Spawner myTarget = (Spawner)target;
        DrawDefaultInspector();
        if(GUILayout.Button("Spawn"))
        {
            myTarget.Generate();
        }

    }
}