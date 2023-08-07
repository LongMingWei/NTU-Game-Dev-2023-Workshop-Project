using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 upForce;
    public float combineUpForce; //For combining force when holding down

    private bool isHoldingJump = false;//Track if the player is holding the button
    public float maxJumpingTime = 0.5f;//The max time the player can hold his jump button

    private float currentMaxJumpingTime = 1.0f;
    private float currentHoldingTime = 0.0f;
    public float boostRatio = 0.35f; //a ratio we use for additional boost when holding mouse
    public float rotationSpeed = 200f; // Adjust the rotation speed as needed

    private Rigidbody2D rb2d;
    public bool isGrounded = false;

    private Animator animator;
    private GameController gameController;
    public GameObject laser;
    // Start is called before the first frame update
    void Start()
    {
        combineUpForce = upForce.y; //the force when we never hold the jump

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameController = GameController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.GetGameOver())
        {
            //Only when we are on the ground then we can jump
            if (isGrounded)
            {
                laser.SetActive(false);
                transform.rotation = Quaternion.identity;
                if (Input.GetButtonDown("Jump"))
                {
                    rb2d.velocity = Vector2.zero; //Reset the velocity (gravity pulling the player)
                    rb2d.AddForce(upForce); //Add a upward force to the player

                    isGrounded = false; //We leave the ground so isGrounded is false
                    animator.SetBool("IsJumping", true); //Set the animation to be jumping

                    combineUpForce = upForce.y; //Reset the combine up force
                    isHoldingJump = true;                
                }
            }
            else
            {
                laser.SetActive(true);
                float horizontalInput = Input.GetAxis("Horizontal");

                // Rotate the player based on the horizontal input
                if (horizontalInput != 0f)
                {
                    // Rotate around the y-axis based on the horizontal input
                    transform.Rotate(Vector3.forward, -horizontalInput * rotationSpeed * Time.deltaTime);
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                //we are no longer holding the jump button
                isHoldingJump = false;
            }

            //If it is falling
            if (rb2d.velocity.y < 0.0f)
            {
                animator.SetBool("IsJumping", false); //we are no longer jumping
                animator.SetBool("IsFalling", true); //we are falling
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.collider.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false); //we are no longer jumping
            animator.SetBool("IsFalling", false); //we are no longer falling, let run
        }

        if(collision.collider.tag == "Dead Zone")
        {
            GameController.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            GameController.instance.SetSpeedMultiplier(0.5f);
            Destroy(collision.gameObject);

            animator.SetTrigger("IsHit");
            GameController.instance.PlayHit();

            StartCoroutine(WaitCoroutine(2f));
        }
    }

    private IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        GameController.instance.SetSpeedMultiplier(1.0f);
    }

    private void FixedUpdate()
    {
        if (!gameController.GetGameOver())
        {
            if (!isGrounded)
            {
                if (isHoldingJump)
                {
                    //Add the time and push the player up slightly
                    currentHoldingTime += Time.fixedDeltaTime;
                    rb2d.AddForce(upForce * boostRatio);
                    combineUpForce += upForce.y * boostRatio;

                    //if we hold longer than the max time to hold, we directly stop the player from holding
                    if (currentHoldingTime >= currentMaxJumpingTime)
                    {
                        isHoldingJump = false;
                        combineUpForce = upForce.y;
                        currentHoldingTime = 0.0f;
                    }
                }
            }
        }
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void SetMaxJumpingTime(float ratio)
    {
        currentMaxJumpingTime = maxJumpingTime * ratio;
    }    

    public float GetCurrentMaxJumpingTime()
    {
        return currentMaxJumpingTime;
    }

    public float GetMaxJumpVelocity()
    {
        float acc = combineUpForce / rb2d.mass;
        float maxJumpVelocity = acc * Time.fixedDeltaTime;

        return maxJumpVelocity;
    }
}
