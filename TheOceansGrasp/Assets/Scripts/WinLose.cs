using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour {
    public GameObject winCamera;
    public GameObject loseCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WinGame()
    {
        winCamera.tag = "MainCamera";
    }

    public void LoseGame()
    {
        loseCamera.tag = "MainCamera";
    }
}
