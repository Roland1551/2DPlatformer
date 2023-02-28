using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    int currentHealth;
    public int maxHealth = 100;
    public float moveSpeed;

    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //minimum distance for attackInit
    
    public float timer; //cooldown between attacks
    private RaycastHit2D hit;
    private GameObject target;
    private float distance; //storing the distance between enemy and player
    private bool attackMode;

    private bool inRange;
    private bool cooling;
    private float intTimer;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        //When player is detected
        if (hit.collider != null)
        {
            Debug.Log("I see player");
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }


        if (inRange == false)
        {
            Debug.Log("I don't see player");
            animator.SetBool("canWalk", false);
            StopAttack();
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        //play damaged animation
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Rigidbody2D Rigid = GetComponent<Rigidbody2D>();
        Debug.Log("Enemy died");

        //death animation
        animator.SetBool("isDead", true);

        //disable enemy and collider

        //disables script
        this.enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
    }
    public void OnTriggerEnter2D(Collider2D trig)
    {
     
        if (trig.gameObject.tag == "Player")
        {
            target = trig.gameObject;
            inRange = true;
           
        }
    }

    //DEALING ATTACK----------------------------------------------
   

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            Move();
            StopAttack();
            
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();

        }

        if (cooling)
        {
            Cooldown();
            animator.SetBool("enemyAttack", false);
        }
    }

    void Move()
    {
        animator.SetBool("canWalk", true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("enemyAttack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        animator.SetBool("canWalk", false);
        animator.SetBool("enemyAttack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;

        }
    }
    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        animator.SetBool("enemyAttack", false);
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    //TAKING DAMAGE AND DEATH -------------------------------------
 
}
