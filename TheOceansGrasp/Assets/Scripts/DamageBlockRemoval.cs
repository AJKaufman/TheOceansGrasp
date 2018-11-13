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
    public bool wrongObject;
    public bool isDamaged;
    public Camera playerCam;
    public Material gray;

	// Use this for initialization
	void Start () {
        distance = 100.0f;
        repairTimer = 0.0f;
        isClicked = false;
        isDamaged = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isDamaged)
        {
            if (isClicked)
            {
                if (Input.GetButton("RepairTool"))
                {
                    RaycastHit hit;

                    if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        if (gameObject != hit.transform.gameObject)
                        {
                            wrongObject = true;
                        }
                        else
                        {
                            wrongObject = false;
                        }
                    }
                    Debug.DrawRay(Input.mousePosition, new Vector3());

                    // calculate the distance between the player and the damage block
                    distance = Vector3.Magnitude(player.GetComponent<Transform>().position - gameObject.transform.position);

                    // if it is within 2 meters
                    if (!wrongObject)
                    {
                        Debug.Log("yeet");
                        // enable the slider canvas to show progress bar
                        Image[] images = slider.GetComponentsInChildren<Image>();
                        foreach (Image image in images)
                        {
                            image.enabled = true;
                        }

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

                            // disable canvas to stop showing progress bar
                            images = slider.GetComponentsInChildren<Image>();
                            foreach (Image image in images)
                            {
                                image.enabled = false;
                            }

                            // then destroy the object after the variables have been reset
                            isDamaged = false;
                            gameObject.GetComponent<MeshRenderer>().material = gray;
                            // Destroy(this.gameObject);
                        }
                    }
                    else
                    {
                        // disable canvas to stop showing progress bar
                        Image[] images = slider.GetComponentsInChildren<Image>();
                        foreach (Image image in images)
                        {
                            image.enabled = false;
                        }

                        // reset timer
                        repairTimer = 0.0f;

                        // resets the progress bar
                        slider.value = 0.0f;
                    }
                }
            }
        }
	}
    private void OnMouseDown()
    {
        Debug.Log("CLICKED");
        isClicked = true;
    }


    // resets the repair timer if they released early
    private void OnMouseUp()
    {
        // reset timer
        repairTimer = 0.0f;

        // disable canvas to stop showing progress bar
        Image[] images = slider.GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            image.enabled = false;
        }

        // resets the progress bar
        slider.value = 0.0f;
    }
}
