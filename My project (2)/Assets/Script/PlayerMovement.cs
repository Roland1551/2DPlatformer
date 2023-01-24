using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private float jumpPower;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D player;
    private Animator CurrentAnim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;


    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        CurrentAnim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Character flipping
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Sets animation parameters
        CurrentAnim.SetBool("Run", horizontalInput != 0);
        CurrentAnim.SetBool("Grounded", IsGrounded());

        //Wall jump
        if (wallJumpCooldown > 0.2f)
        {
            player.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, player.velocity.y);

            if (OnWall() && !IsGrounded())
            {
                player.gravityScale = 0;
                player.velocity = Vector2.zero;
            }
            else
                player.gravityScale = 2;

            if (Input.GetKey(KeyCode.UpArrow))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, jumpPower);
            CurrentAnim.SetTrigger("Jump"); 
        }
        else if (OnWall() && !IsGrounded())
        {
            if(horizontalInput == 0)
            {
                player.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                player.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
                wallJumpCooldown = 0;
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
