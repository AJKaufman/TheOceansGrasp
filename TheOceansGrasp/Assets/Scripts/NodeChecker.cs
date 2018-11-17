using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeChecker : MonoBehaviour {

    public bool isDamaged = false;
    public GameObject normalNode;
    public GameObject damagedNode;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isDamaged)
        {
            normalNode.SetActive(false);
            damagedNode.SetActive(true);
        }
        else
        {
            normalNode.SetActive(true);
            damagedNode.SetActive(false);
        }
	}
}
