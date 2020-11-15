using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet planet;
    private Editor shapeEditor;
    private Editor colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }
        DrawSettingEditor((planet.shapeSettings), planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout,
            ref shapeEditor);
        DrawSettingEditor((planet.colorSettings), planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout,
            ref colorEditor);
    }

    void DrawSettingEditor(Object settings, System.Action onSettingUpdated, ref bool foldOut, ref Editor editor)
    {
        if (settings != null)
        {
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldOut)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (check.changed)
                        {
                            if (onSettingUpdated != null)
                            {
                                onSettingUpdated();
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet) target;
    }
}