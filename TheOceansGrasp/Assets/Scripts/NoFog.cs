using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFog : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnPreRender()
    {
        RenderSettings.fog = false;
    }

    private void OnPostRender()
    {
        RenderSettings.fog = true;
    }
}
