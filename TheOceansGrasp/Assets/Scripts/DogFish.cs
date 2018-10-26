using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFish : SeekerFish {

	[Header("Dog Fish")]
	public float attackRange = 2;
	public float boredomPeriod = 15; // Seconds til the fish goes away

	[Header("Speed")]
	// All multipliers are of the sub max speed
	public float dashSpeedMultiplier = 1.2f; // Except this, this is dash speed
	public float maxSpeedMultiplier = 1.2f;
	public float midSpeedMultiplier = 1;
	public float slowSpeedMultiplier = 0.5f;

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
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
	}
}
