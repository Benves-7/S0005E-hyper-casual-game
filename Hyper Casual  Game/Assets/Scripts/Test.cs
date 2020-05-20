using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("References")]
    public GameObject cube;

    [Header("Rotation - Test Values")]
    public float rotationSpeed;

    [Header("Values calculated")]
    public float currentDegrees;
    public float degreesLeft;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            degreesLeft = 90;
        }
        if (degreesLeft > 0)
        {
            float degrees = rotationSpeed * 360 * Time.deltaTime;
            transform.Rotate(degrees, 0, 0);
            currentDegrees += degrees;
            degreesLeft -= degrees;
        }
        if (degreesLeft < 0)
        {
            transform.Rotate(degreesLeft, 0, 0);
            currentDegrees += degreesLeft;
            degreesLeft = 0;
        }
        if (currentDegrees > 360)
        {
            currentDegrees -= 360;
        }
    }
}
