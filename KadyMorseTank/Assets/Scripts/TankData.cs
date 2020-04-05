using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankData : MonoBehaviour
{
    // sets the variables and what they are equal to also it gives the designers the abality to edit it in inspector
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 180.0f;
    public float reverseSpeed = 1.0f;
    public float shellForce = 1.0f;
    public float damageDone = 1.0f;
    public float fireRate = 1.0f;
    public float health;
    public float maxHealth = 10.0f;
    public int score = 0;
    public int pointValue = 10;
    public int playerNumber = 1;
    public GameManager manager;

    void Start()
    {
        //states that health equals max health
        health = maxHealth;
    }
    public void updateHealth(float shellDamage)
    {
        //the cannon does damage if the health is above zero
        if (health > 0)
        {
            health -= shellDamage;
        }
    }

        public void updateDamageDone(float damage)
    {
        //damage that is done equals to the damage
        damageDone += damage;
    }
        public void TakeDamage(float damage)
    {
        // health is subtracted from damage and if it is less then zeor then tank will die
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //destroys the game object when it dies 
        Destroy(gameObject);
    }
}
