using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOn : MonoBehaviour {

    public GameObject Light;
    bool on = false;
    bool selected = false;
    bool clickedon = false;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && clickedon == false)
        {
            selected = false;
        }
        if (selected && Input.GetKeyDown(KeyCode.L))
        {
            print("sheet");
            if (on)
            {
                Light.SetActive(false);
                on = false;
            }
            else
            {
                Light.SetActive(true);
                on = true;
            }
        }
        clickedon = false;
	}

    void OnMouseDown()
    {
        if (selected == false)
        {
            selected = true;
        }
        clickedon = true;
    }
}
