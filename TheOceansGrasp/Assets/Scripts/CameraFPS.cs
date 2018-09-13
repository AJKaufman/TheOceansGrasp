using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFPS : MonoBehaviour {
    public float FPS = 5f;
    float elapsed;
    bool highfps = false;
    bool selected = false;
    public Camera renderCam;

	// Use this for initialization
	void Start () {
        renderCam.enabled = false;
	}
    

    // Update is called once per frame
    void Update () {
        if (highfps == false)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 1 / FPS)
            {
                elapsed = 0;
                renderCam.Render();
            }
            if (Input.GetButtonDown("Fire2") && selected == true)
            {
                highfps = true;
            }
        }
        else
        {
            renderCam.Render();
            if (Input.GetButtonDown("Fire2") || selected == false)
            {
                highfps = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (selected == false)
        {
            selected = true;
        }
        else
        {
            selected = false;
        }
    }

}
