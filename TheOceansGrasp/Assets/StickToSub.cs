using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSub : MonoBehaviour {

    public GameObject sub;
	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(12, 2);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = sub.transform.position;
        transform.rotation = sub.transform.rotation;
	}
}
