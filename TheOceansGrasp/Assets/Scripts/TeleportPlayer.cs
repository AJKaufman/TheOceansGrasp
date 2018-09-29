using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour {

    public bool inside = true;
    public GameObject submarine;
    public GameObject player;
    private SubmarineMovement subMovement;
    private PlayerCamera playerCamera;
    private PlayerSwim swim;
    private float distanceFromHatch;
    private Transform subTransform;
    private Vector3 subPosition;

	// Use this for initialization
	void Start () {
        subMovement = submarine.GetComponent<SubmarineMovement>();
        subPosition = submarine.transform.position;
        swim = player.GetComponent<PlayerSwim>();
        playerCamera = player.GetComponent<PlayerCamera>();
        subTransform = submarine.GetComponent<Transform>();
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
        // update the position of the submarine's hatch
        //subTransform = submarine.GetComponent<Transform>();
        // update the submarine's position to move the hatch effectively
        subPosition = new Vector3(submarine.GetComponent<Transform>().position.x, submarine.GetComponent<Transform>().position.y, submarine.GetComponent<Transform>().position.z);

        if (inside)
        {
            // adding to it isn't actually moving the position
            player.transform.position = new Vector3(subPosition.x + 0.0f, subPosition.y + 15.0f, subPosition.z + 5.0f);
            inside = false;
            swim.enabled = true;
            playerCamera.enabled = false;
            subMovement.enabled = false;
            //GameObject.FindGameObjectWithTag("Sub").GetComponent<SubmarineMovement>().enabled = false;
        }
        else
        {
            // if the player is close enough to the outer hatch for them to make a reasonable jump
            if(distanceFromHatch <= 2.0f)
            {
                player.transform.position = new Vector3(3000.0f, 100.0f, 2.3f);
                player.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                inside = true;
                swim.enabled = false;
                playerCamera.enabled = true;
                subMovement.enabled = true;
                //GameObject.FindGameObjectWithTag("Sub").GetComponent<SubmarineMovement>().enabled = true;
            }
        }
    }
}
