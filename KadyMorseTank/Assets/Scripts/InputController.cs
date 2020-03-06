using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankShooter))]
public class InputController : MonoBehaviour
{

    //variables of differnt objects and the input is wasd and arrow keys also set the boolean if the tank can shoot 
    public enum InputScheme { WASD, arrowKeys };
    public InputScheme input = InputScheme.WASD;
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;
    private float timeUntilCanShoot;
    public bool canShoot = true;


    // Start is called before the first frame update
    void Start()
    {
        //tells when the game starts what these objects mean 
        data = gameObject.GetComponent<TankData>();
        motor = gameObject.GetComponent<TankMotor>();
        shooter = gameObject.GetComponent<TankShooter>();
    }

    // Update is called once per frame
    void Update()
    {

        //if the player can shoot then it will get the key of space and shootbut if can shoot is false then it will wait to fire
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Shoot
                shooter.Shoot();
                canShoot = false;
                timeUntilCanShoot = data.fireRate;
            }
        }
        //if you are wait if it shoots the time until shoot will subtract from the time till you can unless you can shoot
        // Determine if the player can shoot again yet.
        if (timeUntilCanShoot > 0)
        {
            timeUntilCanShoot -= Time.deltaTime;
        }
        else
        {
            canShoot = true;
        }

        switch (input)
        {
            case InputScheme.arrowKeys:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    motor.Rotate(data.rotateSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    motor.Rotate(-data.rotateSpeed);
                }
                break;
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W))
                {
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    motor.Rotate(data.rotateSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    motor.Rotate(-data.rotateSpeed);
                }

                break;
        }
    }
}
