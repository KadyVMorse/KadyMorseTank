using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    //variables for the cannon ball such as damage the life of the cannon and the public game object
    public float damage;
    public float timer;
    public float cannonShelfLife = 1.5f;
    public GameObject shooter;

    void Start()
    {
        // if damage is equal to 0 then it would be equal to 10
        if (damage == 0)
        {
            damage = 10;
        }
    }
    void Update()
    {
        // After a set amount of time, destroy the cannon ball.//after a certain amount of time bullet disappers
        timer += 1.0F * Time.deltaTime;

        //if timer  is more then 1 theb it will destroy the game object
        if (timer >= 1)
        {
            GameObject.Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // gets the tank data 
        TankData otherObjData = other.gameObject.GetComponent<TankData>();


        if (otherObjData != null)
        {

            otherObjData.updateHealth(damage);


            shooter.GetComponent<TankData>().updateDamageDone(damage);

            // if game object health is equal to zero then it will destroy the object

            if (otherObjData.health <= 0)
            {
                Destroy(other.gameObject);


            }


        }
    }
}
