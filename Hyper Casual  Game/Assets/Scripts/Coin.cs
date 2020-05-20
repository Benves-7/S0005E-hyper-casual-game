using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float RPM;
    public bool pickup;

    public float t; 
    public float time;
    public float dist = 30f;

    public void Update()
    {
        transform.Rotate(Vector3.forward, 360f * Time.deltaTime * RPM);

        if (pickup)
        {
            if (time > 0)
            {
                transform.Translate(Vector3.forward * dist * Time.deltaTime);
                time -= Time.deltaTime;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void PickUp()
    {
        pickup = true;
        time = t;
    }
}