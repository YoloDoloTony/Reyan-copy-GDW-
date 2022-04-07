using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public List<GravHinge> gravHinge;

    public LayerMask ObjectLayer;
    public LayerMask Layer;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip walking;

    //Player Movement
    [SerializeField] public float playerSpeed;
    private Transform _leftFoot;
    private Transform _rightFoot;

    bool facingRight = true;
    bool isWalking = false;
    bool isFlip = false;
    Vector2 movementDir = new Vector2(0.0f, 0.0f);

    public float dashForce;
    public float delayTime;
    private float save;
    private float _walkingSoundTimer = 0.4f;
    Vector2 currentPos;

    Animator animator;

    //Gravity Variables
    bool isGrounded;
    bool flipCooldown = true;

    bool GameRunning = true;
    bool isDead = false;


    public enum ERotationStates
    {
        Up,
        Down,
        Left,
        Right
    }

    private ERotationStates rotationState;

    public void ChangeGravity(ERotationStates newRotation)
    {
        //This is an enum
        rotationState = newRotation;

        switch (rotationState)
        {
            case ERotationStates.Up:
                Physics2D.gravity = new Vector2(0, 9.81f);
                transform.eulerAngles = new Vector3(0, 0, -180);
                for (int i = 0; i < gravHinge.Count; i++)
                {
                    gravHinge[i].TargetRotation(Quaternion.AngleAxis(90, transform.forward));
                }
                break;
            case ERotationStates.Down:
                Physics2D.gravity = new Vector2(0, -9.81f);
                transform.eulerAngles = new Vector3(0, 0, 0);
                for (int i = 0; i < gravHinge.Count; i++)
                {
                    gravHinge[i].TargetRotation(Quaternion.AngleAxis(-90, transform.forward));
                }
                break;
            case ERotationStates.Left:
                Physics2D.gravity = new Vector2(9.81f, 0);
                transform.eulerAngles = new Vector3(0, 0, 90);
                for (int i = 0; i < gravHinge.Count; i++)
                {
                    gravHinge[i].TargetRotation(Quaternion.AngleAxis(0, transform.forward));
                }
                break;
            case ERotationStates.Right:
                Physics2D.gravity = new Vector2(-9.81f, 0);
                transform.eulerAngles = new Vector3(0, 0, -90);
                for (int i = 0; i < gravHinge.Count; i++)
                {
                    gravHinge[i].TargetRotation(Quaternion.AngleAxis(-180, transform.forward));
                }
                break;
        }
    }

    void Start()
    {
        _leftFoot = transform.GetChild(0);
        _rightFoot = transform.GetChild(1);

        ChangeGravity(ERotationStates.Down);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        save = delayTime;
        Time.timeScale = 1;
        GameRunning = true;

        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_walkingSoundTimer > 0)
        {
            _walkingSoundTimer -= Time.deltaTime;
        }
        else
        {
            PlaySound();
            _walkingSoundTimer = 0.4f;
        }

        Debug.DrawRay(transform.position, movementDir * dashForce, Color.green);
        
        MovePlayer();

        if (flipCooldown)
        {
            //Check for Switch Gravity Input
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isFlip = true;
                StartCoroutine(FlipDelay());
            }
        }

        HandleLanding();

        if (Input.GetKeyDown(KeyCode.LeftShift) && AbleToDash())
        {
            Dash();
        }

        animator.SetBool("Isgrounded", isGrounded);
    }


    
    public void HandleLanding()
    {
        
        RaycastHit2D hitLeft = Physics2D.Raycast(_leftFoot.position, -transform.up, 0.75f, Layer);
        RaycastHit2D hitRight = Physics2D.Raycast(_rightFoot.position, -transform.up, 0.75f, Layer);

        Debug.DrawRay(_leftFoot.position, -transform.up * 0.75f, Color.green);
        Debug.DrawRay(_rightFoot.position, -transform.up * 0.75f, Color.green);
        if (hitLeft.collider != null || hitRight.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private IEnumerator FlipCooldown()
    {
        flipCooldown = false;
        yield return new WaitForSeconds(2);
        flipCooldown = true;
    }

    //Player Movement
    private void MovePlayer()
    {
        movementDir = new Vector2(0f, 0f);

        switch(rotationState)
        {
            // player movement for when they are on the ground
            case ERotationStates.Down:

                if (Input.GetKey(KeyCode.A))
                {
                    movementDir += new Vector2(-1.0f, 0.0f);

                    if (facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    movementDir += new Vector2(1.0f, 0.0f);

                    if (!facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else
                {
                    isWalking = false;
                }
                break;
            // player movement for when they are on the roof
            case ERotationStates.Up:

                if (Input.GetKey(KeyCode.D))
                {
                    movementDir += new Vector2(1.0f, 0.0f);

                    if (facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    movementDir += new Vector2(-1.0f, 0.0f);

                    if (!facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else
                {
                    isWalking = false;
                }
                break;
            // player movement for when tehy are on the right wall
            case ERotationStates.Right:

                if (Input.GetKey(KeyCode.A))
                {
                    movementDir += new Vector2(0.0f, -1.0f);

                    if (facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    movementDir += new Vector2(0.0f, 1.0f);

                    if (!facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else
                {
                    isWalking = false;
                }
                break;
            // player movement for when they are on the left wall
            case ERotationStates.Left:

                if (Input.GetKey(KeyCode.D))
                {
                    movementDir += new Vector2(0.0f, 1.0f);

                    if (facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    movementDir += new Vector2(0.0f, -1.0f);

                    if (!facingRight)
                    {
                        FaceDirection();
                    }
                    isWalking = true;
                }
                else
                {
                    isWalking = false;
                }
                break;

        }
        rb.velocity = movementDir * (playerSpeed);
    }

    //Switches what way player is facing
    public void FaceDirection()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    //180 degree Gravity Switch
    void SwitchGravity()
    {
        ERotationStates temp = ERotationStates.Down;

        switch (rotationState)
        {
            case ERotationStates.Up:
                temp = ERotationStates.Down;

                break;
            case ERotationStates.Down:
                temp = ERotationStates.Up;

                break;
            case ERotationStates.Left:
                temp = ERotationStates.Right;

                break;
            case ERotationStates.Right:
                temp = ERotationStates.Left;

                break;
        }

        ChangeGravity(temp);

        isGrounded = false;
    }

    private IEnumerator FlipDelay()
    {
        if (isFlip)
        {
            StartCoroutine(FlipCooldown());
            playerSpeed = 0;
            yield return new WaitForSeconds(1);
            isGrounded = false;
            SwitchGravity();
            yield return new WaitForSeconds(0.05f);

            playerSpeed = 10;
        }
        
    }


    public void Dash()
    {

        currentPos += movementDir * dashForce;
        transform.position = currentPos;
        ResetTimer();
    }

    // checks if the delay is up or not
    public bool CanDash()
    {
        if (delayTime - Time.realtimeSinceStartup < 0)
        {
            if (AbleToDash())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            delayTime -= Time.deltaTime;
            return false;
        }
    }

    //Reset delay timer 
    public void ResetTimer()
    {
        delayTime += Time.realtimeSinceStartup + save;
    }

    private bool AbleToDash()
    {
        if (facingRight)
        {
            return Physics2D.Raycast(rb.transform.position, Vector2.right, dashForce, ObjectLayer).collider == null;
        }
        else
        {
            return Physics2D.Raycast(rb.transform.position, Vector2.right * -1, dashForce, ObjectLayer).collider == null;
        }
    }


    public bool GetIsRunning()
    {
        return GameRunning;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public bool GetIsWalking()
    {
        return isWalking;
    }
    public bool GetIsFlip()
    {
        return isFlip;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFlip = false;


        if (collision.gameObject.CompareTag("Death"))
        {
            isDead = true;
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            transform.parent = null;
        }

        if (collision.gameObject.CompareTag("BulletBilly"))
        {
            Destroy(collision.gameObject);
            isDead = true;
            Time.timeScale = 0;
        }
    }

    private void OnGUI()
    {
        if (isDead)
        {
            GameRunning = false;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 50;
            if (GUI.Button(new Rect(Screen.width / 2 - 350, Screen.height / 2 - 210, 700, 420), "YOU DIED!\n\nClick to Restart", buttonStyle))
            {
                Physics2D.gravity = new Vector2(0, -9.81f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
            }
        }
    }

    void PlaySound()
    {
        if (isWalking)
        {
            _audioSource.PlayOneShot(walking);
        }
    }
}