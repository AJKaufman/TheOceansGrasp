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
    public bool stactive = false;
    public bool highfps = false;
    public bool selected = false;
    public Camera renderCam;

    // For fish
    public bool targeted = false;

	// Use this for initialization
	void Start () {
        renderCam.enabled = false;
        
	}
    

    // Update is called once per frame
    void Update () {
        if (damaged == false)
        {
            if (highfps == false)
            {
                elapsed += Time.deltaTime;
                if (elapsed > 1 / FPS)
                {
                    elapsed = 0;
                    renderCam.Render();
                }
            }
            else
            {
                renderCam.Render();
            }
        }
    }

    /*void OnMouseDown()
    {
        print("selected");
        if (selected == false)
        {
            selected = true;
        }
    }*/

    public void Damage()
    {
        if (damaged)
        {
            broken = true;
        }
        damaged = true;
        stactive = true;
        GetComponent<RawImage>().texture = stat;
    }

    public void Repair()
    {
        if (broken == false)
        {
            damaged = false;
            stactive = false;
            GetComponent<RawImage>().texture = camTex;
        }
    }

    public void HighFPS()
    {
        highfps = !highfps;
    }
}
