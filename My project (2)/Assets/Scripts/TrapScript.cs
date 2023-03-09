using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] public int Damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerCombat Player = collision.gameObject.GetComponent<PlayerCombat>();

        if (Player != null)
        {
            Player.TakeDamage(Damage);
        }
    }
}
