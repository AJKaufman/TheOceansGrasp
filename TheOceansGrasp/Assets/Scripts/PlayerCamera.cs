using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    float sensitivity = 0.05f;
    Vector3 playerCamForward = new Vector3(0.0f,0.0f,0.0f);
    public Camera playerCamera;
    float mouseX;
    float mouseY;

    // Use this for initialization
    void Start ()
    {
        //playerCamera = GetComponent<Camera>();
        //transform.LookAt(playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane)), Vector3.up);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //mouseX = Input.mousePosition.x;
        //mouseY = Input.mousePosition.y;

        // get the forward vector of the camera
        playerCamForward = playerCamera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane));

        // adjust the rotational speed based on the sensistivity value
        playerCamForward.x -= 0.5f;
        playerCamForward.y -= 0.5f;
        playerCamForward.x *= sensitivity;
        playerCamForward.y *= sensitivity;
        playerCamForward.x += 0.5f;
        playerCamForward.y += 0.5f;

        // convert the viewport point to a screen point
        Vector3 screenPoint = playerCamera.ViewportToScreenPoint(playerCamForward);

        // convert the screen point to a world point
        Vector3 worldPoint = playerCamera.ScreenToWorldPoint(screenPoint);

        // have the camera look at the world point
        transform.LookAt(worldPoint, Vector3.up);       
    }
}
