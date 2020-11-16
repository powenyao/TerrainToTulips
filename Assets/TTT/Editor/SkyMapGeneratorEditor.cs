using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkyMapManager))]
public class SkyMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkyMapManager skymapGen = (SkyMapManager)target;
        
        if (DrawDefaultInspector())
        {
            if (skymapGen.autoUpdate)
            {
                skymapGen.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            skymapGen.DrawMapInEditor();
        }
    }
}
