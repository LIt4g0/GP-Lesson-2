using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8;
    public float acceleration = 5;
    public float deceleration = 20;

    [Header("Jump")]
    public float jumpPower = 8;
    public float extraGravity = 4;
    public int maxJumps = 2;

    [Header("Art")]
    public Sprite jumpSprite;
    public Sprite groundSprite;

    public int gems = 0;

    //Private variables
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    float xVelocity;
    bool isGrounded;
    Vector3 feetPosition;
    int currentJumps = 0;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        feetPosition.y -= GetComponent<Collider2D>().bounds.extents.y + 0.01f;
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        AdjustGravity();
        Movement();
        GroundCheck();
        Jump();
    }

    private void AdjustGravity()
    {
        if (rb2D.velocity.y < 0)
            rb2D.gravityScale = extraGravity;
        else
            rb2D.gravityScale = 1;
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        xVelocity += x * acceleration * Time.fixedDeltaTime;
        xVelocity = Mathf.Clamp(xVelocity, -maxSpeed, maxSpeed);

        //(x < 0 == xVelocity > 0)
        //(true/false) == (true/false)
        //if true: then movement input has flipped direction = decelerate
        //if false: then movement input is same direction as previous = do no decelerate
        if (x == 0 || (x < 0 == xVelocity > 0))
        {
            xVelocity *= Mathf.Clamp01(1 - (deceleration * Time.fixedDeltaTime));
        }

        rb2D.velocity = new Vector2(xVelocity, rb2D.velocity.y);

        if (rb2D.velocity.x > 0)
            spriteRenderer.flipX = false;
        else if (rb2D.velocity.x < 0)
            spriteRenderer.flipX = true;
    }

    private void GroundCheck()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + feetPosition, Vector2.down, 0.01f);

        //Debug.DrawRay(transform.position + feetPosition, Vector2.down);

        if (hit.collider != null)
        {
            isGrounded = true;
            currentJumps = 0;
            spriteRenderer.sprite = groundSprite;
        }
        else
        {
            isGrounded = false;
            spriteRenderer.sprite = jumpSprite;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && currentJumps < maxJumps)
        {
            currentJumps++;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb2D.velocity.y > 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Destroy(other.gameObject);
        gems++;
    }
}
