using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHit : MonoBehaviour {

    public float timer = 0.0f;
    public bool hit = false;
    public bool iFrames = false;
    public GameObject sub;
    public GameObject buttonManager;
    private DischargePrompt discharge;

	// Use this for initialization
	void Start () {
        discharge = buttonManager.GetComponent<DischargePrompt>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hit)
        {
            if(iFrames == false)
            {
                sub.GetComponent<SubVariables>().loseHealth(5.0f);
                iFrames = true;

                // play sound
                discharge.PlayPillarSound();

                Invoke("OnDamage", 2.0f);
            }
        }
    }
     

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sub")
        {
            
            sub = collision.gameObject;
            hit = true;
            if(iFrames == false)
            {
                sub.GetComponent<SubVariables>().loseHealth(5.0f);
                iFrames = true;
                Invoke("OnDamage",2.0f);
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Sub")
        {
            //timer = 0;
            hit = false;
        }
    }

    public void OnDamage()
    {
        iFrames = false;
    }
}
