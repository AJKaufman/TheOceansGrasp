using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opacity : MonoBehaviour {

    public float howClear;

	// Use this for initialization
	void Start () {
        Color newColor = new Color(0f, 0f, 0f, howClear);
        gameObject.GetComponent<Renderer>().material.color = newColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
