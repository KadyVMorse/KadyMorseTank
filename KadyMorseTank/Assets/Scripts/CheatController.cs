using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PowerupController))]
public class CheatController : MonoBehaviour
{
    private PowerupController powCon;

    public Powerup cheatPowerup;

    // Start is called before the first frame update
    void Start()
    {
        if (powCon == null)
        {
            powCon = gameObject.GetComponent<PowerupController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            powCon.Add(cheatPowerup);
        }
    }
}
