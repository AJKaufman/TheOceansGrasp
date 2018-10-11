using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RadarBlip : MonoBehaviour {
    float elapsed;
    public Camera radarCam;
    public RawImage rawim;
    float alpha = 1.0f;
    // Use this for initialization
    void Start () {
        radarCam.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;
        if (elapsed > 3)
        {
            elapsed = 0;
            alpha = 1.0f;
            radarCam.Render();
        }
        alpha -= .01f;
        Color curcolor = rawim.color;
        curcolor.a = alpha;
        rawim.color = curcolor;
    }
}
