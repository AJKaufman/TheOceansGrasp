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
    public GameObject camModel;
    public GameObject submarine;
    private SubVariables subVar;

    // For fish
    public bool targeted = false;

	// Use this for initialization
	void Start () {
        renderCam.enabled = false;
        subVar = submarine.GetComponent<SubVariables>();
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
        }
        else
        {
            renderCam.Render();
            subVar.loseEnergy();
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
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            GetComponentInChildren<Cycle>().setAlpha(1.0f);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        Debug.Log("Added damaged camera to list");
        Positions.instance.damagedCameras.Add(camModel);
        damaged = true;
        stactive = true;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Repair()
    {
        Debug.Log("Removed damaged camera from list");
        Positions.instance.damagedCameras.Remove(camModel);
        if (broken == false)
        {
            damaged = false;
            stactive = false;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if (broken)
        {
            broken = false;
            damaged = false;
            stactive = false;
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void HighFPS()
    {
        highfps = !highfps;
    }
}
