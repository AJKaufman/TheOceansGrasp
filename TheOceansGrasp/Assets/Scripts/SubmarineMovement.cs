﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmarineMovement : MonoBehaviour
{

    public Vector3 position;
    private Vector3 velocity = new Vector3(0.0f, 0.0f, 1.0f);

    public float speed = 0.0f;
    private Rigidbody rb;
    public float maxBackSpeed = 0.0f;
    public float speedIncrement = 1.0f;
    public float maxSpeed;
    public float slowDown = 0.97f;
    public float udAngle = 0.0f;
    public float rlAngle = 0.0f;
    public bool useSlowdown = true;
    public bool boosting = false;
    public bool halfSpeed = false;
    public bool quadSpeed = false;
    public bool eightSpeed = false;
    private float boostTimer = 0.0f;
    private SubVariables subVar;

    // Use this for initialization
    void Start()
    {
        subVar = gameObject.GetComponent<SubVariables>();
        // set the starting position to the starting position of the gameobject
        position = transform.position;
        maxSpeed = 5.0f;
        maxBackSpeed = maxSpeed * -1.0f;
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Sub")
        {
            if(subVar.displayedEnergy.value >= 0)
            {
                Debug.Log("Speed: " + speed);
                // these conditionals don't actually do anything while the variables are "public"
                // if boost is toggled, double some values to speed it up
                if (boosting)
                {
                    // drain energy
                    subVar.loseEnergy();
                    subVar.loseEnergy();
                    /*
                    boostTimer += Time.deltaTime;
                    if(boostTimer >= 1.0f)
                    {
                        boostTimer = 0;
                        subVar.loseEnergy(2.0f);
                    }*/

                    maxBackSpeed = -10.0f;
                    maxSpeed = 10.0f;
                    speedIncrement = 2.0f;
                }
                else if (halfSpeed)
                {
                    maxBackSpeed = -2.5f;
                    maxSpeed = 2.5f;
                    speedIncrement = 0.5f;
                }
                else if (quadSpeed)
                {
                    maxBackSpeed = -1.25f;
                    maxSpeed = 1.25f;
                    speedIncrement = 0.25f;
                }
                else if (eightSpeed)
                {
                    maxBackSpeed = -0.625f;
                    maxSpeed = 0.625f;
                    speedIncrement = 0.125f;
                }
                // otherwise convert it back to the normal settings
                else
                {
                    maxSpeed = 5.0f;
                    maxBackSpeed = -5.0f;
                    speedIncrement = 1.0f;
                }

                // when boosting cases
                if (halfSpeed && boosting)
                {
                    maxBackSpeed = -5.0f;
                    maxSpeed = 5.0f;
                    speedIncrement = 1.0f;
                }
                else if (quadSpeed && boosting)
                {
                    maxBackSpeed = -2.5f;
                    maxSpeed = 2.5f;
                    speedIncrement = 0.5f;
                }
                else if (eightSpeed && boosting)
                {
                    maxBackSpeed = -1.25f;
                    maxSpeed = 1.25f;
                    speedIncrement = 0.25f;
                }

                // determine if the sub is moving or not to gain energy or deplete it
                if (Input.GetButton("Forward") || Input.GetButton("Backward") || Input.GetButton("RotateLeft") || Input.GetButton("RotateRight") || Input.GetButton("Ascend") || Input.GetButton("Descend"))
                {
                    subVar.loseEnergy();
                }
                else
                {
                    subVar.gainEnergy();
                }

                // move forward
                if (Input.GetButton("Forward"))
                {
                    //speed += speedIncrement * Time.deltaTime;
                    speed += speedIncrement * speedIncrement * Time.deltaTime;
                    //speed *= speed + (speedIncrement * Time.deltaTime);
                }
                // move backwards
                else if (Input.GetButton("Backward"))
                {
                    //speed -= speedIncrement * Time.deltaTime;
                    //speed *= speed - (speedIncrement * Time.deltaTime);
                    speed -= speedIncrement * speedIncrement * Time.deltaTime;
                }
                // friction
                else if (useSlowdown)
                {
                    speed *= slowDown;
                }
                // clamp speed
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                if (speed < maxBackSpeed)
                {
                    speed = maxBackSpeed;
                }
                else if (speed < 0.01f && speed > -0.01 && !Input.GetButton("Forward") && !Input.GetButton("Backward"))
                {
                    speed = 0.0f;
                }

                // rotate right
                if (Input.GetButton("RotateRight"))
                {
                    rlAngle += 15.0f * Time.deltaTime;
                }
                // rotate left
                else if (Input.GetButton("RotateLeft"))
                {
                    rlAngle -= 15.0f * Time.deltaTime;
                }
                // rotate up
                if (Input.GetButton("Ascend"))
                {
                    udAngle -= 10.0f * Time.deltaTime;
                }
                // rotate down
                else if (Input.GetButton("Descend"))
                {
                    udAngle += 10.0f * Time.deltaTime;
                }
                // clamp angles -- just up and down for now, not left and right
                /*
                if(rlAngle > 45.0f)
                {
                    rlAngle = 45.0f;
                }
                else if(rlAngle < -45.0f)
                {
                    rlAngle = -45.0f;
                }*/
                if (udAngle > 30.0f)
                {
                    udAngle = 30.0f;
                }
                else if (udAngle < -30.0f)
                {
                    udAngle = -30.0f;
                }
                // apply the rotation and position change
                transform.rotation = Quaternion.Euler(udAngle, rlAngle, 0.0f);
                rb.velocity = rb.transform.forward * speed;
                // assign the new position to the object
                //transform.position = position;
            }
        }
    }

    // gets the direction of the object
    public Vector3 GetDirection()
    {
        // get vector of length = 1 unit because we only need direction, not speed or velocity
        return Vector3.Normalize(transform.rotation * velocity);
    }

}
