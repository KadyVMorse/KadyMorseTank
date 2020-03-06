using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankData : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 180.0f;
    public float shellForce = 1.0f;
    public float damageDone = 1.0f;
    public float fireRate = 1.0f;
    public float health;
    public float maxHealth = 10.0f;
    public int score = 0;
    public int pointValue = 10;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // This is what happens when the tank dies.
    }
}
