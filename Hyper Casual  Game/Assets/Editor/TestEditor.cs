using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Coin))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Coin test = (Coin)target;

        if (GUILayout.Button("Test Pickup"))
        {
            test.PickUp();
        }

        base.OnInspectorGUI();
    }
}
