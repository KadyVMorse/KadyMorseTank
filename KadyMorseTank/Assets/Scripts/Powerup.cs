using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
    //defines the powerups,duration,and is perment 
    public float speedModifier;
    public float healthModifier;
    public float maxHealthModifier;
    public float fireRateModifier;

    public float duration;
    public bool isPermanent;

    public void OnActivate(TankData target)
    {
        //if the powerups is activated then it will add that certain powerup depending on what is picked up 
        target.moveSpeed += speedModifier;
        target.health += healthModifier;
        target.maxHealth += maxHealthModifier;
        target.fireRate += fireRateModifier;
    }

    public void OnDeactivate(TankData target)
    {
        //when they lose the powerup then it will be removed from the the player of the AI 
        target.moveSpeed -= speedModifier;
        target.health -= healthModifier;
        target.maxHealth -= maxHealthModifier;
        target.fireRate -= fireRateModifier;
    }
}
