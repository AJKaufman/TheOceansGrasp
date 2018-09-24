using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSwim : MonoBehaviour
{

    [SerializeField] private MouseLook m_MouseLook;

    private Vector3 position;
    public Vector3 velocity = new Vector3(1.0f, 0.0f, 0.0f);
    public Camera playerCamera = new Camera();

    public float speed = 0.0f;
    private float speedIncrement = 5.0f;
    public float maxSpeed = 3.0f;
    private float slowDown = 0.20f;
    private Vector3 forward;
    private Vector3 up;
    private Vector3 right;
    public Vector2 mousePos = new Vector2(0.0f, 0.0f);
    public bool useSlowdown = true;
    public GameObject submarine;

    // Use this for initialization
    void Start()
    {
        // set the starting position to the starting position of the gameobject
        position = new Vector3(submarine.GetComponent<Transform>().position.x, submarine.GetComponent<Transform>().position.y + 6.5f, submarine.GetComponent<Transform>().position.z + 4.0f);
        transform.position = position;
        m_MouseLook.Init(transform, playerCamera.transform);
        forward = new Vector3(1.0f, 0.0f, 0.0f);
        up = gameObject.transform.up;
        right = gameObject.transform.right;
    }

    private void OnEnable()
    {
        // set the starting position to the starting position of the gameobject
        position = new Vector3(submarine.GetComponent<Transform>().position.x, submarine.GetComponent<Transform>().position.y + 6.5f, submarine.GetComponent<Transform>().position.z + 4.0f);
        transform.position = position;
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Player")
        {
            // call the method to rotate the view
            RotateView();

            // calculate the forward, right, and up vectors
            //forward = GetDirection();
            forward = Vector3.forward;
            forward = forward.normalized;
            up = up.normalized;
            right = Vector3.right;
            // cross product of up and forward to get right
            /*
            right = Vector3.Cross(gameObject.transform.forward, gameObject.transform.up);
            right = right.normalized;
            */

            if (Input.GetKey(KeyCode.W))
            {
                //velocity = transform.forward;
                velocity = forward;
                speed += speedIncrement * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                velocity = forward * -1.0f;
                speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity = right * -1.0f;
                speed += speedIncrement * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                velocity = right;
                speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velocity = up * -1.0f;
                speed += speedIncrement * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                velocity = up;
                speed += speedIncrement * Time.deltaTime;
            }
            else if (!Input.GetKey(KeyCode.W) &&
                !Input.GetKey(KeyCode.A) &&
                !Input.GetKey(KeyCode.S) &&
                !Input.GetKey(KeyCode.D) &&
                !Input.GetKey(KeyCode.LeftShift) &&
                !Input.GetKey(KeyCode.Space) &&
                !Input.GetKey(KeyCode.W) && useSlowdown)
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
            /*
            // forward
            forward = gameObject.transform.forward;

            // set the rotation = to the direction
            gameObject.transform.rotation = Quaternion.Euler(0.0f, rlAngle, udAngle);
            direction = Input.mousePosition - gameObject.transform.position;

            // apply the rotation and position change
            position += transform.rotation * velocity * speed * Time.deltaTime;
            // assign the new position to the object
            transform.position = position;
            playerCamera.transform.position = position;
            playerCamera.transform.rotation = Quaternion.Euler(direction.x, direction.y, direction.z);
            */
            position += transform.rotation * velocity * speed * Time.deltaTime;
            transform.position = position;
        }
    }

    // rotates the view
    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, playerCamera.transform);
    }

    // gets the direction of the object
    public Vector3 GetDirection()
    {
        // get vector of length = 1 unit because we only need direction, not speed or velocity
        forward = Vector3.Normalize(transform.rotation * velocity);
        return forward;
    }
}
