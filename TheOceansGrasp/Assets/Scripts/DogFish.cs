using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFish : SeekerFish {

	[Header("Dog Fish")]
	public float attackRange = 2;
	public float boredomPeriod = 15; // Seconds til the fish goes away
    private float boredomTimer = 0;
    private GameObject sub;
    private bool inLight = false;

	[Header("Speed")]
	public float dashSpeedMultiplier = 1.2f; // Of dash speed
	public float maxSpeedMultiplier = 1.2f; // Of max speed
	public float midSpeedMultiplier = 1; // Of current speed
	public float slowSpeedMultiplier = 0.5f; // Of current speed

	public float outsideSpeedMultipler = 0.1f;
	public float lightSpeedMultipler = 0.2f;
	public float returnSpeedMultipler = 0.2f;

	public float fleeSpeedMultipler = 0.5f;

	public float slowSpeedThreshold = 0.5f; // Half max speed

	[Header("Audio")]
	public AudioClip randomSwimAudio;
	public AudioClip maxSpeedAudio;
	public AudioClip dashSpeedAudio;
	public AudioClip attackAudio;
	public AudioClip randomStopAudio;
	public AudioClip fleeAudio;

	public float randomAudioTimer = 15; // Seconds

	// Use this for initialization
	override protected void Start () {
		base.Start();
		damage = 20;

        sub = FindObjectOfType<SubmarineMovement>().gameObject; // get sub object
        Camera[] cams = FindObjectsOfType<Camera>();
        foreach(Camera c in cams)
        {
            if (c.CompareTag("BackCam"))
            {
                targetObject = c.gameObject;
                break;
            }
        }

	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
	}

    private void DetermineMaxSpeed()
    {
        float subMaxSpeed = sub.GetComponent<SubmarineMovement>().maxSpeed;

        if (targetObject.CompareTag("BackCam"))
        {
            float subSpeed = sub.GetComponent<SubmarineMovement>().speed;
            bool bored = false;

            if (subSpeed > subMaxSpeed)
            {
                maxSpeed = subSpeed * dashSpeedMultiplier;
            }
            else if (subSpeed == subMaxSpeed)
            {
                maxSpeed = subSpeed * maxSpeedMultiplier;
            }
            else if (subSpeed > slowSpeedThreshold * subMaxSpeed)
            {
                maxSpeed = subSpeed * midSpeedMultiplier;
            }
            else if (subSpeed <= slowSpeedThreshold * subMaxSpeed && subSpeed > 0)
            {
                maxSpeed = subSpeed * slowSpeedMultiplier;
            }
            else
            {
                maxSpeed = 0;
                bored = true;
                boredomTimer += Time.deltaTime;
                if (boredomTimer >= boredomPeriod)
                {
                    // Do not retarget once fleeing
                    Flee(targetObject);
                    seekPriority = 0;
                }
            }

            if (!bored)
            {
                boredomTimer = 0;
            }
        }
        else if (targetObject.CompareTag("Player"))
        {
            if (inLight)
            {
                maxSpeed = subMaxSpeed * lightSpeedMultipler;
                inLight = false;
            }
            else
            {
                maxSpeed = subMaxSpeed * outsideSpeedMultipler;
            }
        }
    }

    protected override void SeekBehavior()
    {
        if(Vector3.SqrMagnitude(targetObject.transform.position - transform.position) > maxAggroRange * maxAggroRange)
        {
            Destroy(gameObject);
        }
        base.SeekBehavior();
        DetermineMaxSpeed();
    }

    public void SetInLight()
    {
        inLight = true;
    }

    protected override void FleeBehavior()
    {
        maxSpeed = targetObject.GetComponent<SubmarineMovement>().maxSpeed * fleeSpeedMultipler;
        base.FleeBehavior();
    }

}
