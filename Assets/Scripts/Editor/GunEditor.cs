using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Gun), true)]
public class GunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Fire Weapon"))
        {
            (target as Weapon).Fire();
        }
    }
}
