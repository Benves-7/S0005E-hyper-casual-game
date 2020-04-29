using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public float sizeOfSegment;
    public float speed;
    public bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {

    }
}
