using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
public class SampleAIControllerTwo : MonoBehaviour
{
    public enum AttackMode { Chase, Flee }

    public AttackMode attackMode;

    public Transform target;

    private TankData data;

    private TankMotor motor;

    public float fleeDistance = 1.0f;

    private Transform tf;
    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<TankData>();
        motor = gameObject.GetComponent<TankMotor>();
        tf = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackMode)
        {
            case AttackMode.Chase:
                // Rotate towards target
                motor.RotateTowards(target.position, data.rotateSpeed);
                // Move towards target
                motor.Move(data.moveSpeed);
                break;
            case AttackMode.Flee:
                // The vector from ai to target is target position minus our position.
                Vector3 vectorToTarget = target.position - tf.position;

                // We can flip this vector by -1 to get a vector AWAY from our target
                Vector3 vectorAwayFromTarget = -1 * vectorToTarget;

                // Now, we can normalize that vector to give it a magnitude of 1
                vectorAwayFromTarget.Normalize();

                // A normalized vector can be multiplied by a length to make a vector of that length.
                vectorAwayFromTarget *= fleeDistance;

                // We can find the position in space we want to move to by adding our vector away from our AI to our AI's position.
                //     This gives us a point that is "that vector away" from our current position.
                Vector3 fleePosition = vectorAwayFromTarget + tf.position;
                motor.RotateTowards(fleePosition, data.rotateSpeed);
                motor.Move(data.moveSpeed);

                break;
            default:
                Debug.LogError("Attack mode not implemented.");
                break;
        }
    }
}
