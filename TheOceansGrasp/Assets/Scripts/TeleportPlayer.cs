using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPlayer : MonoBehaviour
{

    public bool inside = true;
    public GameObject submarine;
    public GameObject light1;
    public GameObject light2;
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
    private SubVariables subVar;
    private List<GameObject> nodes;
    public RawImage playerCursor;

    // Use this for initialization
    void Start()
    {
        subMovement = submarine.GetComponent<SubmarineMovement>();
        subPosition = submarine.transform.position;
        swim = player.GetComponent<PlayerSwim>();
        lightRen = player.GetComponentInChildren<VolumetricLightRenderer>();
        playerCamera = player.GetComponent<PlayerCamera>();
        subTransform = submarine.GetComponent<Transform>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        subVar = submarine.GetComponent<SubVariables>();
    }

    // Update is called once per frame
    void Update()
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
        //Debug.Log("yeet");
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
            player.transform.position = new Vector3(subPosition.x + 0.0f, subPosition.y + 10.0f, subPosition.z + 5.0f);
            //Debug.Log("SubY: " + subPosition.y);
            //Debug.Log("PlayerY: " + player.transform.position.y);
            swim.enabled = true;
            if (Positions.instance.tooClose == false)
            {
                Positions.instance.ChangeMusic(2);
            }
            playerCamera.enabled = false;
            player.GetComponentInChildren<Light>().enabled = true;
            subMovement.enabled = false;
            lightRen.enabled = true;
            submarine.GetComponent<CapsuleCollider>().enabled = true;
            submarine.GetComponent<SphereCollider>().enabled = true;
            submarine.GetComponent<BoxCollider>().enabled = false;
            light1.GetComponent<Light>().enabled = false;
            light2.GetComponent<Light>().enabled = true;
            submarine.GetComponent<Rigidbody>().isKinematic = true;
            playerRigidbody.velocity = Vector3.zero;
            Positions.instance.outside = true;
            foreach (GameObject node in Positions.instance.damagedNodes)
            {
                node.transform.parent = null;
            }
            foreach (GameObject subCamera in Positions.instance.damagedCameras)
            {
                subCamera.transform.SetParent(null);
                Debug.Log("New camera parent: " + subCamera.transform.parent);
            }
            //GameObject.FindGameObjectWithTag("Sub").GetComponent<SubmarineMovement>().enabled = false;
        }
        else
        {
            //Debug.Log("Doubleyeet");
            // if the player is close enough to the outer hatch for them to make a reasonable jump
            if (distanceFromHatch <= 4.0f)
            {
                Positions.instance.outside = false;
                if(Positions.instance.tooClose == false)
                {
                    Positions.instance.ChangeMusic(0);
                }
                foreach (GameObject node in Positions.instance.damagedNodes)
                {
                    node.transform.parent = Positions.instance.universalParent;
                }
                foreach (GameObject subCamera in Positions.instance.damagedCameras)
                {
                    subCamera.transform.SetParent(Positions.instance.universalParent);
                    Debug.Log("reparented camera");
                }
                gameObject.transform.parent = submarine.transform;
                //otherScript.inside = !otherScript.inside;
                //inside = !inside;
                light1.GetComponent<Light>().enabled = true;
                light2.GetComponent<Light>().enabled = false;
                submarine.GetComponent<CapsuleCollider>().enabled = false;
                submarine.GetComponent<SphereCollider>().enabled = false;
                submarine.GetComponent<BoxCollider>().enabled = true;
                player.transform.position = new Vector3(3000.0f, 100.0f, 1.19f);
                player.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                swim.enabled = false;
                player.GetComponentInChildren<Light>().enabled = false;
                playerRigidbody.velocity = Vector3.zero;
                playerCamera.enabled = true;
                subMovement.enabled = true;
                submarine.GetComponent<Rigidbody>().isKinematic = false;
                //GameObject.FindGameObjectWithTag("Sub").GetComponent<SubmarineMovement>().enabled = true;
                lightRen.enabled = false;
            }
        }
    }

    // make the cursor change color on mouse over
    private void OnMouseOver()
    {
        playerCursor.GetComponent<RawImage>().color = Color.green;
    }
    private void OnMouseExit()
    {
        playerCursor.GetComponent<RawImage>().color = Color.white;
    }
}