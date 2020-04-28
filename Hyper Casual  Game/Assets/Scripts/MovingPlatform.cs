﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float testVariable1;
    public float testVariable2;
    public bool testVariable3;

    public float speedX;
    public float speedZ;

    public float maxX;
    public float minX;

    public float maxZ;
    public float minZ;

    public bool goingRight;
    public bool goingForward;

    public bool stop = false;

    private void Start()
    {
        maxZ = transform.localPosition.z + maxZ;
        minZ = transform.localPosition.z + minZ;
    }

    private void Update()
    {
        testVariable1 = transform.localPosition.z;
        testVariable2 = transform.localPosition.x;
        testVariable3 = transform.localPosition.z >= maxZ;

        if (!stop)
        {
            if (goingRight)
            {
                transform.Translate(Vector3.right * speedX * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * -speedX * Time.deltaTime);
            }

            if (goingForward)
            {
                transform.Translate(Vector3.forward * speedZ * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * -speedZ * Time.deltaTime);
            }

            if (transform.localPosition.x >= maxX)
            {
                goingRight = false;
            }
            if (transform.localPosition.x <= minX)
            {
                goingRight = true;
            }
            if (transform.localPosition.z >= maxZ)
            {
                goingForward = false;
            }
            if (transform.localPosition.z <= minZ)
            {
                goingForward = true;
            }
        }
    }
}