using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerFish2 : SeekerFish {

    private enum PlayerSeek
    {
        Hunt, Wait, Attack
    };
    private PlayerSeek playerSeekStatus = PlayerSeek.Hunt;

    [Header("Advanced Seeker Fish")]
    public float attackWait = 3;

    public float attackDistance = 15;

    public float seekSpeedModifier = 0.5f; // of sub
    public float huntSpeedModifier = 0.5f; // of swim
    public float attackSpeedModifier = 1.0f; // of swim
    public float fleeSpeedModifier = 1.5f; // of sub

    public float weakAvoidance;
    public float strongAvoidance;

    private SubmarineMovement sub;
    private PlayerSwim swimmer;


    // Use this for initialization
    override protected void Start () {
        base.Start();

        sub = FindObjectOfType<SubmarineMovement>();
        swimmer = FindObjectOfType<PlayerSwim>();
	}
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
	}

    override protected void SeekBehavior()
    {
        maxSpeed = seekSpeedModifier * sub.maxSpeed;
        if (targetObject.CompareTag("Player"))
        {
            maxSpeed = huntSpeedModifier * sub.maxSpeed;
            switch (playerSeekStatus)
            {
                case PlayerSeek.Hunt:
                    if(Vector3.SqrMagnitude(targetObject.transform.position - transform.position) < attackDistance * attackDistance)
                    {
                        playerSeekStatus = PlayerSeek.Wait;
                    }
                    break;

                case PlayerSeek.Wait:
                    attackWait -= Time.deltaTime;
                    maxSpeed = 0;
                    break;

                case PlayerSeek.Attack:
                    // Fish should no longer turn during its lunge
                    targetPosition = transform.forward * 5;
                    maxSpeed = attackSpeedModifier * swimmer.maxSpeed;
                    break;
            }
        }

        base.SeekBehavior();
    }

    protected override void FleeBehavior()
    {
        maxSpeed = fleeSpeedModifier * sub.maxSpeed;
        base.FleeBehavior();
    }

    protected override void WanderBehavior()
    {
        maxSpeed = seekSpeedModifier * sub.maxSpeed;
        targetPosition = sub.gameObject.transform.position - new Vector3(0, 0, 300);
    }
}
