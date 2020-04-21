using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float JumpForce;
    public bool isGrounded;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.5f;

    public float MoveSpeed;

    public Rigidbody rigidbody;
    public float size;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        size = transform.localScale.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        JumpForce = 4f;
        MoveSpeed = 10f;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            rigidbody.velocity = Vector3.up * JumpForce;
            isGrounded = false;
        }
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Movement
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, 0f);
        }
    }
    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.tag == "Map")
        {
            isGrounded = true;
        }
    }
}
