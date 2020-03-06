using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class PowerupController : MonoBehaviour
{
    //defines the list of the differnt powerups and the tankdata 
    public List<Powerup> powerups;
    

    private TankData data;
    // Start is called before the first frame update
    void Start()
    {
        //defines the variables 
        powerups = new List<Powerup>();
        data = gameObject.GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        //lsist the power up and time duration 
        List<Powerup> expiredPowerups = new List<Powerup>();

        foreach (Powerup power in powerups)
        {
            //the duration of the power up will minus the time 
            power.duration -= Time.deltaTime;

            //if the powerup time equals zeo then they can recive new power up 
            if (power.duration <= 0)
            {
                expiredPowerups.Add(power);
            }
        }

        //when the powerup duration runs out it will remove it 
        foreach (Powerup power in expiredPowerups)
        {
            power.OnDeactivate(data);
            powerups.Remove(power);
        }
        //it will clear the expired powerups 

        expiredPowerups.Clear();
    }

    public void Add(Powerup powerup)
    {
    //if the powerup is permant it will add it to the player of the AI
        powerup.OnActivate(data);
        if (!powerup.isPermanent)
        {
            powerups.Add(powerup);
        }
    }
}
