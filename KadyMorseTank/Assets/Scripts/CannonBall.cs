using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float damage;

    void Update()
    {
        // After a set amount of time, destroy the cannon ball.
    }

    void OnCollisionEnter(Collision collision)
    {
        // Apply damage to what we collided with.

        // Destroy the cannon ball if it collides with anything
        Destroy(gameObject);
    }
}
