using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
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
        minZ = transform.localPosition.z - minZ;
        maxX = transform.localPosition.x + maxX;
        minX = transform.localPosition.x - minX;
    }

    private void Update()
    {
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
        if (GetComponentInParent<MapLoader>().stop)
        {
            stop = true;
        }
        else
        {
            stop = false;
        }
    }
}
