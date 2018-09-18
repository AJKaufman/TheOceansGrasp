using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PlayerCamera : MonoBehaviour {
    float sensitivity = 0.05f;
    Vector3 playerCamForward = new Vector3(0.0f,0.0f,0.0f);
    public Camera playerCamera;
    float screenCenterX;
    float screenCenterY;
    float mouseX;
    float mouseY;
    float distanceX;
    float distanceY;

    //[DllImport("user32.dll")]
    //static extern bool SetCursorPos(float X, float Y);

    // Use this for initialization
    void Start ()
    {
        // get the center of the screen
        //screenCenterX = Camera.current.pixelWidth / 2.0f;
        //screenCenterY = Camera.current.pixelHeight / 2.0f;

        //screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        //playerCamera = GetComponent<Camera>();
        //transform.LookAt(playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane)), Vector3.up);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // get the mouse positions
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        // calculate the distance this frame
        distanceX = mouseX - screenCenterX;
        distanceY = mouseY - screenCenterY;

        // get the forward vector of the camera
        playerCamForward = playerCamera.ScreenToViewportPoint(new Vector3(distanceX, distanceY, playerCamera.nearClipPlane));

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
        playerCamera.transform.LookAt(worldPoint, Vector3.up);
        //transform.LookAt(worldPoint, Vector3.up);

        // reset the mouse position to the center of the screen
        Input.mousePosition.Set(screenCenterX,screenCenterY,0.0f);
        //SetCursorPos(mouseX, mouseY);
    }
}
