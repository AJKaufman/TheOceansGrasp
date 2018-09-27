using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour {

    public Vector3 position;
    private Vector3 velocity = new Vector3(0.0f, 0.0f, 1.0f);

    public float speed = 0.0f;
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
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag == "Sub")
        {
            if (Input.GetButton("Vertical"))
            {
                speed += speedIncrement * Time.deltaTime;
            }
            /*
            else if(Input.GetButtonDown("Vertical"))
            {
                //velocity *= -1.0f;
            }*/
            else if (useSlowdown)
            {
                speed *= slowDown;
            }

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            else if (speed < 0.01f)
            {
                speed = 0.0f;
            }

            if (Input.GetButton("RotateRight"))
            {
                rlAngle -= 90.0f * Time.deltaTime;
            }
            else if (!Input.GetButton("RotateLeft"))
            {
                rlAngle += 90.0f * Time.deltaTime;
            }
            if(Input.GetButton("Ascend"))
            {
                udAngle += 90.0f * Time.deltaTime;
            }
            /*
            else if (!Input.GetButtonDown("Ascend"))
            {
                udAngle -= 90.0f * Time.deltaTime;
            }*/
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
