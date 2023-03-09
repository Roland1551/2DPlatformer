using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int PlayerHealth = 100;
    public MenuScript menuscript;
    public Text healthText;
        
    //Update is called once per frame
    void Update()
    {
        healthText.text = PlayerHealth.ToString();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;
        animator.SetTrigger("PlayerHurt");

        if (PlayerHealth <= 0)
        {
            animator.SetBool("PlayerIsDead", true);
            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        Destroy(gameObject);
        menuscript.isAlive = false;
    }

    void Attack()
    {
        //Attack Animation
        animator.SetTrigger("Player_attack");

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        //Damage
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log(enemy.name + " damaged!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


