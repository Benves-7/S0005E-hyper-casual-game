using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // values for charactercontroller.
    public Vector3 moveDirection;
    private CharacterController controller;

    // jump values.
    public float JumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public float test;

    // movement values.
    public float MoveSpeed;

    // wall slide values.
    public bool onWall;
    public float slideMultiplier;

    // needed bools (not implemented stuff.)
    public bool isDead;
    public bool finished;
    public int points;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1)
        {
            isDead = true;
        }
        if (!isDead)
        {
            if (finished)
            {

                // GAME FINISH AND SCORE SCREEN

                GameObject cam = GameObject.FindGameObjectWithTag("Camera");
                cam.SetActive(false);
                Map map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
                map.stop = true;
            }


            // Movement
            moveDirection.x = Input.GetAxis("Horizontal") * MoveSpeed;

            // Jump
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                moveDirection.y = JumpForce;
            }
            // Gravity
            else if (!controller.isGrounded)
            {
                if (onWall && moveDirection.y < 0)
                {
                    test = Physics.gravity.y * (slideMultiplier - 1);
                    moveDirection.y += Physics.gravity.y * (slideMultiplier - 1) * Time.deltaTime;
                }
                else if (moveDirection.y <= 0)
                {
                    test = Physics.gravity.y * (fallMultiplier - 1);
                    moveDirection.y += Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                else if (moveDirection.y > 0 && Input.GetButton("Jump"))
                {
                    test = Physics.gravity.y * (lowJumpMultiplier - 1);
                    moveDirection.y += Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
                else
                {
                    test = Physics.gravity.y;
                    moveDirection.y += Physics.gravity.y * Time.deltaTime;
                }
            }

            if (controller.isGrounded)
            {
                onWall = false;
            }

            // Execute move
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            GameObject cam = GameObject.FindGameObjectWithTag("Camera");
            cam.SetActive(false);
            Map map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
            map.stop = true;
        }
    }
    public void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Trap")
        {
            isDead = true;
        }
        if (collision.gameObject.tag == "Wall")
        {
            onWall = true;
        }
        if (collision.gameObject.tag == "Goal")
        {
            finished = true;
        }
        if (collision.gameObject.tag == "Coin")
        {
            points++;
            collision.gameObject.SetActive(false);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //onWall = false;
        }
    }
    public void OnValidate()
    {
        if (slideMultiplier < 1)
        {
            slideMultiplier = 1;
        }
        if (fallMultiplier < 1)
        {
            fallMultiplier = 1;
        }
        if (lowJumpMultiplier < 1)
        {
            lowJumpMultiplier = 1;
        }
    }

}
