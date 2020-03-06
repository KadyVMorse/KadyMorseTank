using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankShooter))]
public class SampleAIControllerFinal : MonoBehaviour
{
    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest, Patrol }

    public enum Personalities { Inky, Pinky, Blinky, Clay }

    public Personalities personality = Personalities.Inky;

    private TankData data;

    public GameObject player;

    private TankMotor motor;

    private Transform tf;

    private TankShooter shooter;

    public AIState aiState = AIState.Chase;
    private float stateEnterTime;
    private float healthRegenPerSecond = 25f;

    public float hearingDistance = 25.0f;
    public float FOVAngle = 45.0f;
    public float inSightsAngle = 10.0f;

    public Transform target;
    public float fleeDistance = 1.0f;

    public enum AvoidanceStage { None, Rotate, Move };

    public AvoidanceStage avoidanceStage;

    public float avoidanceTime = 2.0f;

    private float exitTime;

    public Transform[] waypoints;
    public int currentWaypoint = 0;
    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType = LoopType.Stop;
    public float closeEnough = 1.0f;
    public bool isPatrolForward = true;




    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<TankData>();
        motor = gameObject.GetComponent<TankMotor>();
        tf = gameObject.GetComponent<Transform>();
        shooter = gameObject.GetComponent<TankShooter>();
        target = GameManager.instance.instantiatedPlayerTank.transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (personality)
        {
            case Personalities.Inky:
                Inky();
                break;
            case Personalities.Blinky:
                Blinky();
                break;
            case Personalities.Pinky:
                Pinky();
                break;
            case Personalities.Clay:
                Clay();
                break;
            default:
                break;
        }
    }

    private void Clay()
    {
        // TODO: Implement a finite state machine for this personality
        //throw new NotImplementedException();
    }

    private void Pinky()
    {
        // TODO: Implement a finite state machine for this personality
        throw new NotImplementedException();
    }

    private void Blinky()
    {
        // TODO: Implement a finite state machine for this personality
        throw new NotImplementedException();
    }

    private bool playerIsInRange()
    {
        // Check to see if the player is close enough to shoot AND if we are aimed in the player's direction
        return true;
    }

    private void Chase(GameObject targetGameObject)
    {
        // TODO: Integrate obstacle avoidance
        
        motor.RotateTowards(target.position, data.rotateSpeed);

        // only move if we can.
        if (CanMove(data.moveSpeed))
        {
            motor.Move(data.moveSpeed);
        }
        else
        {
            // Rotate until we can move
            avoidanceStage = AvoidanceStage.Rotate;
        }
    }

    private void Inky()
    {
        switch (aiState)
        {
            case AIState.Chase:
                // Do the state behaviors
                Chase(player);
                // Check for transitions in order of priority
                if (data.health < (data.maxHealth * 0.5))
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (playerIsInRange())
                {
                    ChangeState(AIState.ChaseAndFire);
                }
                break;
            case AIState.ChaseAndFire:
                // Do the state behaviors
                Chase(player);
                Shoot();
                // Check for transitions in the order of priority
                if (data.health < (data.maxHealth * 0.5))
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (!playerIsInRange())
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.CheckForFlee:
                if (playerIsInRange())
                {
                    ChangeState(AIState.Flee);
                }
                else
                {
                    ChangeState(AIState.Rest);
                }
                break;
            case AIState.Flee:
                Flee(player);
                // Wait 30 seconds then check for flee.
                if (Time.time >= (stateEnterTime + 30f))
                {
                    ChangeState(AIState.CheckForFlee);
                }
                break;
            case AIState.Rest:
                Rest();
                if (playerIsInRange())
                {
                    ChangeState(AIState.Flee);
                }
                else if (Mathf.Approximately(data.health, data.maxHealth))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            default:
                break;
        }
    }

    private void Rest()
    {
        // Heal some hitpoints every second.
        data.health += healthRegenPerSecond * Time.deltaTime;
    }

    private void Flee(GameObject player)
    {
        // TODO: Integrate obstacle avoidance
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
    }

    private void Shoot()
    {
        shooter.Shoot();
    }

    public void ChangeState(AIState newState)
    {

        // Change our state
        aiState = newState;

        // save the time we changed states
        stateEnterTime = Time.time;
    }

    public bool CanMove(float speed)
    {
        RaycastHit hit;
        if (Physics.Raycast(tf.position, tf.forward, out hit, speed))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
    }

    private void Avoid()
    {
        if (avoidanceStage == AvoidanceStage.Rotate)
        {
            motor.Rotate(data.rotateSpeed);
            if (CanMove(data.moveSpeed))
            {
                avoidanceStage = AvoidanceStage.Move;
                exitTime = avoidanceTime;
            }
        }

        if (avoidanceStage == AvoidanceStage.Move)
        {
            if (CanMove(data.moveSpeed))
            {
                exitTime -= Time.deltaTime;
                motor.Move(data.moveSpeed);

                if (exitTime <= 0.0f)
                {
                    avoidanceStage = AvoidanceStage.None;
                }
            }
            else
            {
                avoidanceStage = AvoidanceStage.Rotate;
            }

        }
    }

    void Patrol()
    {
        if (motor.RotateTowards(waypoints[currentWaypoint].position, data.rotateSpeed))
        {
            // Do nothing!
        }
        else
        {
            // Move forward
            motor.Move(data.moveSpeed);
        }
        // If we are close to the waypoint,
        //if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < closeEnough)
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough * closeEnough))
        {
            switch (loopType)
            {
                case LoopType.Stop:
                    // Advance to the next waypoint, if we are still in range
                    if (currentWaypoint < waypoints.Length - 1)
                    {
                        currentWaypoint++;
                    }
                    break;
                case LoopType.Loop:
                    if (currentWaypoint < waypoints.Length - 1)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        currentWaypoint = 0;
                    }
                    break;
                case LoopType.PingPong:
                    if (isPatrolForward)
                    {
                        if (currentWaypoint < waypoints.Length - 1)
                        {
                            currentWaypoint++;
                        }
                        else
                        {
                            isPatrolForward = false;
                            currentWaypoint--;
                        }
                    }
                    else
                    {
                        if (currentWaypoint > 0)
                        {
                            currentWaypoint--;
                        }
                        else
                        {
                            isPatrolForward = true;
                            currentWaypoint++;
                        }
                    }
                    break;
                default:
                    Debug.LogError("Loop type not implemented.");
                    break;
            }


        }

    }
}
