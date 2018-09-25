using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraFPS : MonoBehaviour {
    public float FPS = 5f;
    float elapsed;
    public Texture stat;
    public RenderTexture camTex;
    public bool damaged = false;
    public bool broken = false;
    bool stactive = false;
    bool highfps = false;
    bool selected = false;
    public Camera renderCam;

	// Use this for initialization
	void Start () {
        renderCam.enabled = false;
	}
    

    // Update is called once per frame
    void Update () {
        if (damaged == false && stactive == false)
        {
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
        else if (stactive == false)
        {
                GetComponent<RawImage>().texture = stat;
                stactive = true;
        }
        else
        {
            stactive = false;
            GetComponent<RawImage>().texture = camTex;
        }
    }

    void OnMouseDown()
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

    void Damage()
    {
        if (damaged == false)
        {
            damaged = true;
        }
        else if (broken == false)
        {
            broken = true;
        }
    }

    void Repair()
    {
        if (broken == false)
        {
            damaged = false;
        }
    }
}
