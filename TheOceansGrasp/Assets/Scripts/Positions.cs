﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour {

    public static Positions instance = null;
    public List<Vector3> positions = new List<Vector3>();
    public int whatTile = 0;
    public bool outside = false;
    public List<GameObject> damagedNodes = new List<GameObject>();
    public List<GameObject> damagedCameras = new List<GameObject>();
    public Transform universalParent;
    public GameObject lose;
    public GameObject player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Lose()
    {
        lose.SetActive(true);
        player.SetActive(false);
    }
}
