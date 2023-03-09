using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Public Variables
    int currentHealth;
    public int maxHealth = 100;
    public Animator animator;
    public float moveSpeed;
    public float attackDistance; //minimum distance for attackInit
    public float timer; //cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    #endregion

    #region Private Variables
    private float distance; //storing the distance between enemy and player
    private bool attackMode;
    private bool cooling;
    private float intTimer;
    #endregion

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        SelectTarget(); //selects the left or rigth limit
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!attackMode){
            Move();
        }

        if (!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_attack")){
            SelectTarget();
        }

        if (inRange){
            EnemyLogic();
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

    //DEALING ATTACK----------------------------------------------
   

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
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
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
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

    public void TriggerCooling()
    {
        cooling = true;
    }


    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x; 
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else 
        {
            target = rightLimit;
        }

        Flip();
    }
    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }
    //TAKING DAMAGE AND DEATH -------------------------------------
 
}
