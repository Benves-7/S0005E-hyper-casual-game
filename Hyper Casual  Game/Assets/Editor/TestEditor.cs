using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{   
    public override void OnInspectorGUI()
    {
        Test test = (Test)target;

        if (GUILayout.Button("Test"))
        {
            test.runTest();
        }

        base.OnInspectorGUI();
    }
}
