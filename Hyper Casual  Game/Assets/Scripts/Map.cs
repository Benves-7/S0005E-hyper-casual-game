﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float speed = 5f;
    public bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
           
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