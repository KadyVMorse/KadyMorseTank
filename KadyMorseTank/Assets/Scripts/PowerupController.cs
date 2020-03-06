using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class PowerupController : MonoBehaviour
{
    public List<Powerup> powerups;
    

    private TankData data;
    // Start is called before the first frame update
    void Start()
    {
        powerups = new List<Powerup>();
        data = gameObject.GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Powerup> expiredPowerups = new List<Powerup>();

        foreach (Powerup power in powerups)
        {
            power.duration -= Time.deltaTime;

            if (power.duration <= 0)
            {
                expiredPowerups.Add(power);
            }
        }

        foreach (Powerup power in expiredPowerups)
        {
            power.OnDeactivate(data);
            powerups.Remove(power);
        }

        expiredPowerups.Clear();
    }

    public void Add(Powerup powerup)
    {
        powerup.OnActivate(data);
        if (!powerup.isPermanent)
        {
            powerups.Add(powerup);
        }
    }
}
