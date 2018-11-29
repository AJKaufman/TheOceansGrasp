using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBlockRemoval : MonoBehaviour {

    // needs a reference to the player object
    public GameObject player;
    public GameObject damagedNode;
    public Canvas canvas;
    public Slider slider;
    public float distance;
    public float repairTimer;
    public bool isClicked;
    public bool wrongObject;
    public bool isDamaged;
    public bool isCloseEnough;
    public Camera playerCam;
    public Material gray;
    private Transform parent;
    public LayerMask mask;
    public GameObject submarine;

	// Use this for initialization
	void Start () {
        distance = 100.0f;
        repairTimer = 0.0f;
        isClicked = false;
        isDamaged = false;
        parent = gameObject.transform.parent;
        Physics.IgnoreCollision(submarine.GetComponent<BoxCollider>(), gameObject.GetComponent<BoxCollider>());
        Physics.IgnoreCollision(submarine.GetComponent<CapsuleCollider>(), gameObject.GetComponent<BoxCollider>());
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
                    
                    if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, 5, 1<<mask))
                    {
                        distance = hit.distance;
                        //Debug.Log("HIT");
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                        if (gameObject != hit.transform.gameObject)
                        {
                            wrongObject = true;
                        }
                        else
                        {
                            wrongObject = false;
                        }
                    }

                    //GetDistanceToCollider();

                    // calculate the distance between the player and the damage block
                    // distance = Vector3.Magnitude(player.GetComponent<Transform>().position - gameObject.transform.position);
                    //if(distance <= 5.0f)
                    if(isCloseEnough)
                    {
                        if (!wrongObject)
                        {
                            //Debug.Log("yeet");
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

                                Debug.Log("Slider Disabled");
                                // then destroy the object after the variables have been reset
                                isDamaged = false;
                                reParent();
                                Positions.instance.damagedNodes.Remove(gameObject);
                                gameObject.GetComponent<MeshRenderer>().material = gray;
                                // Destroy(this.gameObject);
                            }
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

    void GetDistanceToCollider()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position) - gameObject.GetComponent<Renderer>().bounds.extents.x;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCloseEnough = true;
    }

    // this method is a lil' bitch
    private void OnMouseDown()
    {
        //Debug.Log("CLICKED");
        isClicked = true;
    }

    private void OnMouseExit()
    {
        isClicked = false;

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

    public void reParent()
    {
        gameObject.transform.SetParent(parent);
    }
}
