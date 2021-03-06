﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFish : SeekerFish {

	[Header("Dog Fish")]
	public float attackRange = 2;
    public float damageRange = 1;

    public float boredomPeriod = 15; // Seconds til the fish goes away
    private float boredomTimer = 0;
    private GameObject sub;
    private bool inLight = false;
    private bool didAttack = false;

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
    private AudioSource audioSource;

	private float randomAudioTimer; // Seconds
    public float randomAudioTime = 15;
    public float randomDeviation = 5;
    public float randomOftenAudioTime = 4;
    public float randomOftenDeviation = 2;

    // Use this for initialization
    override protected void Start () {
		base.Start();

        sub = FindObjectOfType<SubmarineMovement>().gameObject;
        targetObject = sub; // get sub object
        audioSource = GetComponentInChildren<AudioSource>();
        ResetAudioTimer();
        FindObjectOfType<SubFishSpawner>().DogSpawned = true;
    }
	
	// Update is called once per frame
	override protected void Update () {
        randomAudioTimer -= Time.deltaTime;
		base.Update();
	}

    private void ResetAudioTimer(bool oftenTimer = false)
    {
        if (oftenTimer)
        {
            randomAudioTimer = randomOftenAudioTime + Random.Range(0, randomOftenDeviation);
        }
        else
        {
            randomAudioTimer = randomAudioTime + Random.Range(0, randomDeviation);
        }
    }

    //TODO: Fix the constant audio playing? This may not be an issue.
    private void DetermineMaxSpeed()
    {
        bool bored = false;
        float subMaxSpeed = sub.GetComponent<SubmarineMovement>().maxSpeed;

        if (targetObject.CompareTag("Sub"))
        {
            float subSpeed = Mathf.Abs(targetObject.GetComponent<SubmarineMovement>().speed);

            if (subSpeed > subMaxSpeed)
            {
                maxSpeed = subSpeed * dashSpeedMultiplier;

                // Play loud panting and barking
                if (audioSource.clip != dashSpeedAudio || !audioSource.isPlaying)
                {
                    audioSource.clip = dashSpeedAudio;
                    audioSource.Play();
                }
            }
            else if (subSpeed == subMaxSpeed)
            {
                maxSpeed = subSpeed * maxSpeedMultiplier;

                // Play panting
                if (audioSource.clip != maxSpeedAudio || !audioSource.isPlaying)
                {
                    audioSource.clip = maxSpeedAudio;
                    audioSource.Play();
                }
            }
            else if (subSpeed > slowSpeedThreshold * subMaxSpeed)
            {
                maxSpeed = subSpeed * midSpeedMultiplier;

                //Might need to stop loop audio here

                // Play quick breath
                if (randomAudioTimer < 0)
                {
                    audioSource.PlayOneShot(randomSwimAudio);
                    ResetAudioTimer();
                }
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
                    seekPriority = int.MinValue;
                }

                // Play curious yip
                if (randomAudioTimer < 0)
                {
                    audioSource.PlayOneShot(randomStopAudio);
                    ResetAudioTimer();
                }
            }
        }
        else if (targetObject.CompareTag("Player"))
        {
            audioSource.Stop();
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
        else
        {
            maxSpeed = subMaxSpeed * fleeSpeedMultipler;
        }

        if (!bored)
        {
            boredomTimer = 0;
        }
    }

    protected override void SeekBehavior()
    {
        if (targetObject)
        {
            if (Vector3.SqrMagnitude(targetObject.transform.position - transform.position) > maxAggroRange * maxAggroRange)
            {
                if (targetObject.CompareTag("Player"))
                {
                    seekPriority = int.MaxValue;
                }
                else
                {
                    Kill();
                }
            }

            targetPosition = targetObject.transform.position;
        }
        //base.SeekBehavior();
        DetermineMaxSpeed();
    }

    public void SetInLight()
    {
        inLight = true;
    }

    protected override void FleeBehavior()
    {
        maxSpeed = sub.GetComponent<SubmarineMovement>().maxSpeed * fleeSpeedMultipler;
        base.FleeBehavior();

        // Play sad yip
        if(randomAudioTimer < 0)
        {
            audioSource.PlayOneShot(fleeAudio);
            ResetAudioTimer(true);
        }
    }

    private CameraFPS GetCameraFPS(Camera cam)
    {
        CameraFPS fps = null;

        CameraFPS[] cameraCanvases = FindObjectsOfType<CameraFPS>();
        foreach (CameraFPS c in cameraCanvases)
        {
            if (c.renderCam == cam)
            {
                fps = c;
                break;
            }
        }

        return fps;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (behaviour == FishBehaviour.Seek) {
            if (other.gameObject == targetObject)
            {
                if (other.CompareTag("Sub") && !didAttack)
                {
                    other.GetComponentInChildren<SubVariables>().loseHealth(damage);
                    didAttack = true;

                    // Damage nearby cameras
                    Camera[] cams = other.GetComponentsInChildren<Camera>();
                    foreach (Camera c in cams)
                    {
                        if ((c.transform.position - other.ClosestPoint(transform.position)).sqrMagnitude < damageRange * damageRange)
                        {
                            GetCameraFPS(c).Damage();
                        }
                    }

                    // Play bark
                    audioSource.PlayOneShot(attackAudio);
                    Flee(targetObject);
                }
                else if (other.CompareTag("Player"))
                {
                    // End the game somehow
                    FindObjectOfType<Positions>().Lose();

                    // Play bark
                    audioSource.PlayOneShot(attackAudio);
                }
            }
        }
    }

    public override void Flee(GameObject fleeFrom)
    {
        base.Flee(fleeFrom);
        ResetAudioTimer(true);
    }

    public override void Kill()
    {
        FindObjectOfType<SubFishSpawner>().DogSpawned = false;
        base.Kill();
    }

    protected override bool IsTarget(string tag)
    {
        if (tag == "fish")
        {
            return true;
        }
        else
        {
            return base.IsTarget(tag);
        }
    }

}
