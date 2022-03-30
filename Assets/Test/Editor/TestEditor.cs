using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Testing))]
public class TestEditor : Editor
{
    Testing testing;
    public void OnEnable()
    {
        testing = target as Testing;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Check"))
        {
            testing.Test();
        }
    }
}
