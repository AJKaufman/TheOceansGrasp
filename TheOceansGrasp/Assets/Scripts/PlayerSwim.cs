using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwim : MonoBehaviour {

    public Vector3 position;
    public Vector3 velocity = new Vector3(-1.0f, 0.0f, 0.0f);

    public float speed = 0.0f;
    public float speedIncrement = 1.0f;
    public float maxSpeed = 5.0f;
    public float slowDown = 0.97f;
    public Vector2 mousePos = new Vector2(0.0f, 0.0f);
    public Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
    public float udAngle = 0.0f;
    public float rlAngle = 0.0f;
    public bool useSlowdown = true;

    // Use this for initialization
    void Start()
    {
        // set the starting position to the starting position of the gameobject
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Sub")
        {
            if (Input.GetKey(KeyCode.W))
            {
                speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity = new Vector3(0.0f, 0.0f, -1.0f);
                speed += speedIncrement * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                velocity = new Vector3(0.0f, 0.0f, 1.0f);
                speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velocity = new Vector3(0.0f, -1.0f, 0.0f);
                speed += speedIncrement * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                velocity = new Vector3(0.0f, 1.0f, 0.0f);
                speed += speedIncrement * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //velocity *= -1.0f;
            }
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
            // apply the rotation and position change
            transform.rotation = Quaternion.Euler(0.0f, rlAngle, udAngle);
            position += transform.rotation * velocity * speed * Time.deltaTime;
            // assign the new position to the object
            transform.position = position;
        }
    }

    // gets the direction of the object
    public Vector3 GetDirection()
    {
        // get vector of length = 1 unit because we only need direction, not speed or velocity
        direction = Vector3.Normalize(transform.rotation * velocity);
        return direction;
    }
}
