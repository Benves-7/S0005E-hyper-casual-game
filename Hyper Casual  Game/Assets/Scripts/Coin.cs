using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float RPM;

    public void Update()
    {

        transform.Rotate(Vector3.forward, 360f * Time.deltaTime * RPM);
    }
}