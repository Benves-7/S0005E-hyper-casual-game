using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // values for charactercontroller.
    public Vector3 moveDirection;

    // jump values.
    public float JumpForce;

    // gravity values
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float slideMultiplier;

    // movement values.
    public float MoveSpeed;

    // wall slide values.
    public bool onWall;
    public bool wallJump;
    public bool leftSide;

    // needed bools (not implemented stuff.)
    public bool stop;
    public int state;
    public int points;

    public string current_tag;


    // References.
    private CharacterController controller;
    private MovingPlatform platform;
    private GameObject cam;
    private RaycastHit hit;
    public MapLoader ml;


    // Debug.DrawRay(transform.position, Vector3.down * 0.35f, Color.black);

    // Set all references.
    void Awake()
    {
        moveDirection = new Vector3(0, 0, 0);
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("Camera");
        ml = GameObject.FindObjectOfType<MapLoader>().GetComponent<MapLoader>();
    }
    // Endscreen
    void EndScreen(int state)
    {
        cam.SetActive(false);
        ml.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if falling of map.
        if (transform.position.y < -0.5f)
        {
            state = 0;
            stop = true;
        }

        if (transform.position.z < -0.5f)
        {
            state = 0;
            stop = true;
        }

        // If Alive..
        if (!stop)
        {
            // Raycast to find surface and check if surface is goal.
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.35f))
            {
                platform = hit.collider.GetComponent<MovingPlatform>();
                if (hit.collider.tag == "Goal")
                {
                    state = 1;
                    stop = true;
                }
            }
            else
            {
                platform = null;
            }

            // Raycast to find surface and check if wall.
            if ((Physics.Raycast(transform.position, Vector3.left, out hit, 0.4f) || Physics.Raycast(transform.position, Vector3.right, out hit, 0.4f)) && hit.collider.tag == "Wall")
            {
                onWall = true;
            }
            else
            {
                onWall = false;
            }

            // Movement
            moveDirection.x = Input.GetAxis("Horizontal") * MoveSpeed;

            // Move with the platform.
            if (platform)
            {
                if (platform.goingRight)
                {
                    moveDirection.x += platform.speedX;
                }
                else
                {
                    moveDirection.x -= platform.speedX;
                }
            }

            // Jump
            if (Input.GetButtonDown("Jump") && (controller.isGrounded || (onWall && wallJump)))
            {
                if (controller.isGrounded)
                {
                    wallJump = true;
                }

                if (onWall && wallJump)
                {
                    wallJump = false;
                    moveDirection.y = JumpForce*0.5f;
                }
                else
                {
                    moveDirection.y = JumpForce;
                }
            }

            // Gravity
            else if (!controller.isGrounded)
            {
                // if on a wall and going down, fall slower then usual.
                if (onWall && moveDirection.y <= 0)
                {
                    moveDirection.y += Physics.gravity.y * (slideMultiplier - 1) * Time.deltaTime;
                }
                // if falling, fall a little faster then gravity (to get a nicer jump, like mario)
                else if (moveDirection.y <= 0)
                {
                    moveDirection.y += Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                // if spacebar is pressed as the character moves upwards, gravity should have less of a effect (higher jump).
                else if (moveDirection.y > 0 && Input.GetButton("Jump"))
                {
                    moveDirection.y += Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
                // if spacebar is not pressed and the character moves upwards, gravity is slowing you down just like normal.
                else
                {
                    moveDirection.y += Physics.gravity.y * Time.deltaTime;
                }
            }


            var deltaMove = moveDirection * Time.deltaTime;
            deltaMove.z = -transform.position.z;
            // Execute move
            controller.Move(deltaMove);
        }
        else
        {
            EndScreen(state);
        }
    }
    public void OnCollisionEnter(Collision collision) 
    {
        current_tag = collision.gameObject.tag;

        if (collision.gameObject.tag == "Trap")
        {
            state = 0;
            stop = true;
        }
        if (collision.gameObject.tag == "Coin")
        {
            points++;
            collision.gameObject.SetActive(false);
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
