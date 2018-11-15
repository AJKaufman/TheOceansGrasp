using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerFish2 : SeekerFish {

    private enum PlayerSeek
    {
        Seek, Wait, Attack
    };
    private PlayerSeek playerSeekStatus = PlayerSeek.Seek;

    [Header("Advanced Seeker Fish")]
    public float attackWait = 3;

    public float attackDistance = 5;

    public float seekSpeed;
    public float attackSpeed;
    public float fleeSpeed;


    // Use this for initialization
    override protected void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
	}

    override protected void SeekBehavior()
    {
        maxSpeed = seekSpeed;
        if (targetObject.CompareTag("Player"))
        {
            switch (playerSeekStatus)
            {
                case PlayerSeek.Seek:
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
                    maxSpeed = attackSpeed;
                    break;
            }
        }

        base.SeekBehavior();
    }

    protected override void FleeBehavior()
    {
        maxSpeed = fleeSpeed;
        base.FleeBehavior();
    }
}
