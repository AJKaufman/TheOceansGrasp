using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityStandardAssets.Characters.FirstPerson;

// just rips the unity fps character controller's camera component because it's better than anything I could make
public class PlayerCamera : MonoBehaviour {

    [SerializeField] private MouseLook m_MouseLook;
    public Camera playerCamera;

    // Use this for initialization
    void Start ()
    {
        m_MouseLook.Init(transform, playerCamera.transform);
    }

    // rotates the view
    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, playerCamera.transform);
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    // Update is called once per frame
    void Update ()
    {
        if(gameObject.tag == "Player")
        {
            RotateView();
        }
    }
}
