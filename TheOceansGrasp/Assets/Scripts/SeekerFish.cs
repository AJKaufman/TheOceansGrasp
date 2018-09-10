using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SeekerFish : MonoBehaviour {

    public float maxSpeed;
    public float maxTurnRate;//Should be in degrees per second

    public Vector3 Velocity { get; private set; }
    // Public for testing
    public Vector3 targetPosition;
    //TODO:  Not actually used yet
    private GameObject targetObject;

    private CharacterController controller;

    // Use this for initialization
    void Start () {
        Velocity = Vector3.forward * maxSpeed;
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (targetObject)
        {
            targetPosition = targetObject.transform.position;
        }

        // Rotate the object to face its target
        Vector3 rotation = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, Mathf.Deg2Rad * maxTurnRate * Time.deltaTime, 1);
        transform.rotation = Quaternion.LookRotation(rotation);

        //TODO: Update to use acceleration/arriving? This currently overshoots the target.
        Velocity = transform.forward * maxSpeed;
        controller.Move(Velocity * Time.deltaTime);
	}
}
