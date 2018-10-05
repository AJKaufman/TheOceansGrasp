using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour {

    public Vector3 position;
    private Vector3 velocity = new Vector3(0.0f, 0.0f, 1.0f);

    public float speed = 0.0f;
    private float maxBackSpeed = 0.0f;
    public float speedIncrement = 1.0f;
    public float maxSpeed = 5.0f;
    public float slowDown = 0.97f;
    public float udAngle = 0.0f;
    public float rlAngle = 0.0f;
    public bool useSlowdown = true;

	// Use this for initialization
	void Start () {
        // set the starting position to the starting position of the gameobject
        position = transform.position;
        maxBackSpeed = maxSpeed * -1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag == "Sub")
        {
            // move forward
            if (Input.GetButton("Forward"))
            {
                //speed += speedIncrement * Time.deltaTime;
                speed += speedIncrement * speedIncrement * Time.deltaTime;
                //speed *= speed + (speedIncrement * Time.deltaTime);
            }
            // move backwards
            else if(Input.GetButton("Backward"))
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
            if(speed < maxBackSpeed)
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
            if(Input.GetButton("Ascend"))
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
            if(udAngle > 30.0f)
            {
                udAngle = 30.0f;
            }
            else if(udAngle < -30.0f)
            {
                udAngle = -30.0f;
            }
            // apply the rotation and position change
            transform.rotation = Quaternion.Euler(udAngle, rlAngle, 0.0f);
            position += transform.rotation * velocity * speed * Time.deltaTime;
            // assign the new position to the object
            transform.position = position;
        }
    }

    // gets the direction of the object
    public Vector3 GetDirection()
    {
        // get vector of length = 1 unit because we only need direction, not speed or velocity
        return Vector3.Normalize(transform.rotation * velocity);
    }
}
