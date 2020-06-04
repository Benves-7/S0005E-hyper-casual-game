using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Values for charactercontroller")]
    public Vector3 moveDirection;

    [Header("Jump values")]
    public float rotationSpeed;
    private AudioSource _audio;
    private float jumpForce = 3;
    private float degreesLeft;
    private float time;

    [Header("Gravity values")]
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 1.5f;
    private float slideMultiplier = 1.25f;

    [Header("Movement values")]
    private float MoveSpeed = 7.5f;

    [Header("Wallrun values.")]
    public bool onWall;
    public bool wallJump;

    [Header("Bools")]
    public bool stop;
    public bool dead;

    [Header("Public State and Point")]
    public int state;
    public int points;

    [Header("References")]
    private CharacterController controller;
    private MovingPlatform platform;
    private RaycastHit hit;
    private Transform cubeTransform;


    // Set all references.
    void Awake()
    {
        moveDirection = new Vector3(0, 0, 0);
        controller = GetComponent<CharacterController>();
        cubeTransform = transform.GetChild(0);
        _audio = GameObject.Find("LevelSFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Death checks.
        if (!dead)
        {
            // Check if falling of map.
            if (transform.position.y < -0.5f)
            {
                state = 0;
                dead = true;
            }
            // Check if stuck behind object.
            if (transform.position.z < -0.5f)
            {
                state = 0;
                dead = true;
            }
        }
        // If not stoped or dead.
        if (!stop && !dead)
        {
            // Raycast to find surface and check if surface is goal.
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.35f))
            {
                platform = hit.collider.GetComponent<MovingPlatform>();
                if (hit.collider.tag == "Goal")
                {
                    state = 1;
                    dead = true;
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

            // Jump button pressed and can jump.
            if (Input.GetButton("Jump") && (controller.isGrounded || (onWall && wallJump)))
            {
                if (controller.isGrounded)
                {
                    wallJump = true;
                    moveDirection.y = jumpForce;
                    print("Jumping from ground");
                    _audio.Play();
                }
                else if (Input.GetButtonDown("Jump"))
                {
                    wallJump = false;
                    moveDirection.y = jumpForce * 0.5f;
                    print("Jumping from wall");
                    _audio.Play();
                }
                if (degreesLeft == 0)
                {
                    degreesLeft = 180;
                }
            }
            // Gravity
            if (!controller.isGrounded)
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
                else if (moveDirection.y > 0 && Input.GetButton("Jump") && !(onWall))
                {
                    moveDirection.y += Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
                // if spacebar is not pressed and the character moves upwards, gravity is slowing you down just like normal.
                else
                {
                    moveDirection.y += Physics.gravity.y * Time.deltaTime;
                }
            }
            if (controller.isGrounded)
            {
                if (time > .5f)
                {
                    time = 0;
                    moveDirection.y = 0;
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
            // Calculate Rotation.
            var deltaMove = moveDirection * Time.deltaTime;
            deltaMove.z = -transform.position.z;
            // Execute movement.
            controller.Move(deltaMove);
            // Execute rotation.
            if (degreesLeft > 0)
            {
                float degrees = rotationSpeed * 360 * Time.deltaTime;
                cubeTransform.Rotate(degrees, 0, 0);
                degreesLeft -= degrees;
            }
            else if (degreesLeft < 0)
            {
                cubeTransform.Rotate(degreesLeft, 0, 0);
                degreesLeft = 0;
            }
        }
    }

    public void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Trap")
        {
            state = 0;
            dead = true;
        }
        if (collision.gameObject.tag == "Coin")
        {
            points += 10;
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<Coin>().PickUp();
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
