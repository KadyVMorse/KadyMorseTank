using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
public class SampleAIControllerThree : MonoBehaviour
{
    public Transform target;
    private TankMotor motor;
    private TankData data;
    private Transform tf;

    public enum AvoidanceStage { None, Rotate, Move };

    public AvoidanceStage avoidanceStage;
    public float avoidanceTime = 2.0f;

    private float exitTime;
    public enum AttackMode { Chase };
    public AttackMode attackMode;

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
        if (attackMode == AttackMode.Chase)
        {
            if (avoidanceStage != AvoidanceStage.None)
            {
                Avoid();
            }
            else
            {
                Chase();
            }
        }
    }

    private void Chase()
    {
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
}
