using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Range(0, 10)]
    public float scale;
    public void runTest()
    {
        Time.timeScale = scale;
    }
}
