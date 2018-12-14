using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBlockRemoval : MonoBehaviour {

    // needs a reference to the player object
    public GameObject player;
    private GameObject repairTool;
    private Animator repairToolAnim;
    //public GameObject damagedNode;
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
    private SubVariables subVar;
    public GameObject cameraFPSObject;
    private CameraFPS camFPS;
    public RawImage playerCursor;
    private bool cursorColorSwap;

	// Use this for initialization
	void Start () {
        cursorColorSwap = false;
        camFPS = cameraFPSObject.GetComponent<CameraFPS>();
        subVar = submarine.GetComponent<SubVariables>();
        repairTool = player.transform.GetChild(4).gameObject;
        repairToolAnim = repairTool.GetComponent<Animator>();
        distance = 100.0f;
        repairTimer = 0.0f;
        isClicked = false;
        isDamaged = false;
        parent = gameObject.transform.parent;
        if(gameObject.tag == "SubCam")
        {
            Physics.IgnoreCollision(submarine.GetComponent<BoxCollider>(), gameObject.GetComponent<SphereCollider>());
            Physics.IgnoreCollision(submarine.GetComponent<CapsuleCollider>(), gameObject.GetComponent<SphereCollider>());
        }
        else
        {
            Physics.IgnoreCollision(submarine.GetComponent<BoxCollider>(), gameObject.GetComponent<BoxCollider>());
            Physics.IgnoreCollision(submarine.GetComponent<CapsuleCollider>(), gameObject.GetComponent<BoxCollider>());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // if it is tagged as a camera, call the camera method
        if(gameObject.tag == "SubCam")
        {
            RepairCamera();
        }
        // otherwise it is a tech node
        else
        {
            RepairDamagedNode();
        }
	}

    // this method is for repairing cameras
    private void RepairCamera()
    {
        if(camFPS.damaged)
        {
            Debug.Log("Camera is damaged");
            gameObject.transform.GetChild(5).gameObject.SetActive(false);
            // try to get the meshes of the camera parts to change color to see if we are even doing damage to the cameras

            /*
            List<MeshRenderer> camModelParts = new List<MeshRenderer>();
            for(int i = 0; i < 7; i++)
            {
                camModelParts.Add(gameObject.transform.GetChild(i).GetComponent<MeshRenderer>());
            }
            foreach(MeshRenderer child in camModelParts)
            {
                child.material.color = Color.red;
                Debug.Log("LoopCount");
            }*/

            //Debug.Log("is Clicked: " + isClicked);
            if(isClicked)
            {
                Debug.Log("Camera Clicked");
                if(Input.GetButton("RepairTool"))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, 5, 1 << mask))
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

                    if(isCloseEnough)
                    {
                        // set the maxValue of the slider so that it shows the progress accurately for the new 8 seconds instead of just 2
                        slider.maxValue = camFPS.repairTimer;
                        if (!wrongObject)
                        {

                            // Play repair animation
                            repairToolAnim.SetFloat("Repairing", 1.0f);

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
                            if (repairTimer >= camFPS.repairTimer)
                            {

                                // End repair animation
                                repairToolAnim.SetFloat("Repairing", 0.0f);

                                camFPS.Repair();
                                gameObject.transform.GetChild(5).gameObject.SetActive(true);
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
                                // Destroy(this.gameObject);
                            }
                        }
                    }
                    else
                    {
                        cursorColorSwap = false;

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

    // this method is for repairing nodes
    private void RepairDamagedNode()
    {
        if (isDamaged)
        {
            if (isClicked)
            {
                if (Input.GetButton("RepairTool"))
                {
                    RaycastHit hit;

                    if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, 5, 1 << mask))
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
                    if (isCloseEnough)
                    {
                        if (!wrongObject)
                        {

                            // Play repair animation
                            repairToolAnim.SetFloat("Repairing", 1.0f);

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

                                // End repair animation
                                repairToolAnim.SetFloat("Repairing", 0.0f);

                                // increment the repair counter in sub variables
                                subVar.totalRepairsMade++;
                                subVar.totalDamageNodes--;

                                // repair/revert system break
                                if (subVar.totalRepairsMade >= 4)
                                {
                                    subVar.systemBreak = false;
                                    subVar.totalRepairsMade = 0;
                                    subVar.damageBeforeSystemBreak = 0.0f;
                                }

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

    // Trigger works, not Rigidbody
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("IN RANGE");
            isCloseEnough = true;
            cursorColorSwap = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("OUT OF RANGE");
            isCloseEnough = false;
            cursorColorSwap = false;
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("IN RANGE");
            isCloseEnough = true;
        }
    }*/

    // this method is a lil' bitch
    private void OnMouseDown()
    {
        Debug.Log("CLICKED");
        isClicked = true;
    }

    private void OnMouseExit()
    {
        isClicked = false;

        // End repair animation
        repairToolAnim.SetFloat("Repairing", 0.0f);

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

        // reset cursor color
        playerCursor.GetComponent<RawImage>().color = Color.white;
    }

    // resets the repair timer if they released early
    private void OnMouseUp()
    {
        // reset timer
        repairTimer = 0.0f;

        // End repair animation
        repairToolAnim.SetFloat("Repairing", 0.0f);

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

    // make the cursor change color on mouse over
    private void OnMouseOver()
    {
        if(cursorColorSwap)
        {
            playerCursor.GetComponent<RawImage>().color = Color.red;
        }
        else
        {
            playerCursor.GetComponent<RawImage>().color = Color.white;
        }
    }
}
