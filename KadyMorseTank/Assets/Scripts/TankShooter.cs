using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject cannonBall;

    public GameObject firePoint;

    private TankData data;


    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Shoot()
    {
        // Instantiate a bullet
        GameObject ourCannonBall = Instantiate(cannonBall, firePoint.transform.position, firePoint.transform.rotation);
        CannonBall cannonBallComponent = ourCannonBall.GetComponent<CannonBall>();
        Rigidbody cannonBody = ourCannonBall.GetComponent<Rigidbody>();

        // Apply Force
        cannonBody.AddForce(data.shellForce * transform.forward, ForceMode.Impulse);

        // Tell the cannon ball how much damage it should do.
        cannonBallComponent.damage = data.damageDone;
    }
}
