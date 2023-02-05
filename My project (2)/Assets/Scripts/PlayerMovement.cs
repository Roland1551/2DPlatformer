using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private float jumpPower;
    private Rigidbody2D Player;
    private Animator anim;
    private bool Grounded;

    /*/////////////////////////////////////AWAKE///////////////////////////////////////////*/
    private void Awake()
    {
        Player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    /*/////////////////////////////////////UPDATE///////////////////////////////////////////*/
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Player.velocity = new Vector2(horizontalInput * speed, Player.velocity.y);

        //Character flipping
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKey(KeyCode.UpArrow) && Grounded)
            Jump();


        //Sets animation parameters in unity
        anim.SetBool("Player_run", horizontalInput != 0);
        anim.SetBool("Grounded", Grounded);
    }

    /*/////////////////////////////////////JUMP///////////////////////////////////////////*/
    private void Jump()
    {
        Player.velocity = new Vector2(Player.velocity.x, jumpPower);
            anim.SetTrigger("Player_jump");
            Grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            Grounded = true; //sets animation parameter
    }
}
