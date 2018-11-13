using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOn : MonoBehaviour {

    public GameObject Light;
    bool on = false;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    /*void OnMouseDown()
    {
        if (selected == false)
        {
            selected = true;
        }
        clickedon = true;
    }*/

    public void TurnOn()
    {
        on = !on;
        if (on)
        {
            Light.GetComponent<Light>().enabled = true;
            Light.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            Light.GetComponent<Light>().enabled = false;
            Light.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
