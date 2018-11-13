using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Cycle : MonoBehaviour {
    public Texture[] statAr;
    private int timer = 0;
    private int current = 0;
    private float transparency = .5f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        timer++;
        if(timer > 5)
        {
            if(current == 8)
            {
                current = 0;
            }
            else
            {
                current++;
            }
            timer = 0;
            GetComponent<RawImage>().texture = statAr[current];
            Color col = GetComponent<RawImage>().color;
            col.a = transparency;
            GetComponent<RawImage>().color = col;
        }
	}

    public void setAlpha(float a)
    {
        transparency = a;
    }
}
