using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBlockRemoval : MonoBehaviour {

    // needs a reference to the player object
    public GameObject player;
    public Canvas canvas;
    public Slider slider;
    public float distance;
    public float repairTimer;
    public bool isClicked;

	// Use this for initialization
	void Start () {
        distance = 100.0f;
        repairTimer = 0.0f;
        isClicked = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isClicked)
        {
            if (Input.GetButton("RepairTool"))
            {
                // calculate the distance between the player and the damage block
                distance = Vector3.Magnitude(player.GetComponent<Transform>().position - gameObject.transform.position);

                // if it is within 2 meters
                if (distance <= 2.0f)
                {
                    // enable the canvas so that it's elements can be seen
                    canvas.GetComponent<Canvas>().enabled = true;

                    // start incrementing the timer
                    repairTimer += Time.deltaTime;

                    // set the slider to equal the timer
                    slider.value = repairTimer;

                    // if the mouse has been held down for 2 seconds
                    if (repairTimer >= 2.0f)
                    {
                        // reset the canvas and slider first
                        repairTimer = 0.0f;
                        slider.value = 0.0f;
                        canvas.GetComponent<Canvas>().enabled = false;

                        // then destroy the object after the variables have been reset
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    // enable the canvas so that it's elements can be seen
                    canvas.GetComponent<Canvas>().enabled = false;

                    // reset timer
                    repairTimer = 0.0f;

                    // resets the progress bar
                    slider.value = 0.0f;
                }
            }
        }
	}

    private void OnMouseDown()
    {
        isClicked = true;
    }


    // resets the repair timer if they released early
    private void OnMouseUp()
    {
        // reset timer
        repairTimer = 0.0f;

        // disable canvas to stop showing progress bar
        canvas.GetComponent<Canvas>().enabled = false;

        // resets the progress bar
        slider.value = 0.0f;
    }
}
