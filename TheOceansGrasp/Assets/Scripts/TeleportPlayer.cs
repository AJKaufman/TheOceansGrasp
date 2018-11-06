using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour {

    public bool inside = true;
    public GameObject submarine;
    public GameObject player;
    public TeleportPlayer otherScript;
    private SubmarineMovement subMovement;
    private PlayerCamera playerCamera;
    private PlayerSwim swim;
    private VolumetricLightRenderer lightRen;
    public float distanceFromHatch;
    private Transform subTransform;
    private Vector3 subPosition;
    private Rigidbody playerRigidbody;

	// Use this for initialization
	void Start () {
        subMovement = submarine.GetComponent<SubmarineMovement>();
        subPosition = submarine.transform.position;
        swim = player.GetComponent<PlayerSwim>();
        lightRen = player.GetComponentInChildren<VolumetricLightRenderer>();
        playerCamera = player.GetComponent<PlayerCamera>();
        subTransform = submarine.GetComponent<Transform>();
        playerRigidbody = player.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!inside)
        {
            // calculate the distance from the hatch
            distanceFromHatch = Vector3.Magnitude(player.transform.position - gameObject.transform.position);
        }
	}

    // teleport the player when they click on this object
    void OnMouseDown()
    {
        Debug.Log("yeet");
        // update the position of the submarine's hatch
        //subTransform = submarine.GetComponent<Transform>();
        // update the submarine's position to move the hatch effectively
        subPosition = new Vector3(submarine.GetComponent<Transform>().position.x, submarine.GetComponent<Transform>().position.y, submarine.GetComponent<Transform>().position.z);
        if (inside)
        {
            // adding to it isn't actually moving the position
            //otherScript.inside = !otherScript.inside;
            //inside = !inside;
            otherScript.gameObject.transform.parent = null;
            player.transform.position = new Vector3(subPosition.x + 0.0f, subPosition.y + 100.0f, subPosition.z + 5.0f);
            Debug.Log("SubY: " + subPosition.y);
            swim.enabled = true;
            playerCamera.enabled = false;
            subMovement.enabled = false;
            lightRen.enabled = true;
            submarine.GetComponent<Rigidbody>().isKinematic = true;
            playerRigidbody.velocity = Vector3.zero;
            //GameObject.FindGameObjectWithTag("Sub").GetComponent<SubmarineMovement>().enabled = false;
        }
        else
        {
            Debug.Log("Doubleyeet");
            // if the player is close enough to the outer hatch for them to make a reasonable jump
            if(distanceFromHatch <= 4.0f)
            {
                gameObject.transform.parent = submarine.transform;
                //otherScript.inside = !otherScript.inside;
                //inside = !inside;
                player.transform.position = new Vector3(3000.0f, 100.0f, 1.19f);
                player.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                swim.enabled = false;
                playerCamera.enabled = true;
                subMovement.enabled = true;
                submarine.GetComponent<Rigidbody>().isKinematic = false;
                //GameObject.FindGameObjectWithTag("Sub").GetComponent<SubmarineMovement>().enabled = true;
                lightRen.enabled = false;
            }
        }
    }
}
