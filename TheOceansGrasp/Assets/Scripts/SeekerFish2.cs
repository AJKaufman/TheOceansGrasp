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

    public float weakAvoidance = 0.1f;
    public float strongAvoidance = 2.0f;

    private SubmarineMovement sub;
    private PlayerSwim swimmer;

    [Header("Audio")]
    private AudioSource audioPlayer;

    [Range(0, 100)]
    public float randomAudioChancePerFrame = 1;

    public AudioClip randomGrowl;
    public AudioClip lowGrowl;
    public AudioClip loudScream;
    public AudioClip loudGrowl;


    // Use this for initialization
    override protected void Start () {
        base.Start();

        audioPlayer = GetComponentInChildren<AudioSource>();
        avoidanceScale = weakAvoidance;
        sub = FindObjectOfType<SubmarineMovement>();
        swimmer = FindObjectOfType<PlayerSwim>();
	}
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
	}

    protected override void SetSeekTarget(GameObject seekTarget, bool willFlee = false)
    {
        base.SetSeekTarget(seekTarget, willFlee);
        if(seekTarget == swimmer.gameObject)
        {
            audioPlayer.PlayOneShot(lowGrowl);
        }
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
                        audioPlayer.PlayOneShot(loudScream);
                    }
                    break;

                case PlayerSeek.Wait:
                    attackWait -= Time.deltaTime;
                    maxSpeed = 0;
                    if(attackWait <= 0)
                    {
                        playerSeekStatus = PlayerSeek.Attack;
                    }
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
        if (targetObject == swimmer)
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y, sub.transform.position.z + 300);
        }
        else
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y, sub.transform.position.z - 300);
        }
        base.FleeBehavior();
    }

    protected override void WanderBehavior()
    {
        maxSpeed = seekSpeedModifier * sub.maxSpeed;
        targetPosition = new Vector3(transform.position.x, transform.position.y, sub.transform.position.z - 300);
        if (!audioPlayer.isPlaying && Random.Range(0, 100f) <= randomAudioChancePerFrame)
        {
            audioPlayer.PlayOneShot(randomGrowl);
        }
    }

    public override void Flee(GameObject fleeFrom)
    {
        avoidanceScale = strongAvoidance;
        audioPlayer.PlayOneShot(loudGrowl);//Is this supposed to be continuous or not?
        base.Flee(fleeFrom);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Sub"))
        {
            collision.gameObject.GetComponent<SubVariables>().loseHealth(damage);
            Flee(collision.gameObject);
        }

        if (targetObject && targetObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<Positions>().Lose();
            }
        }
    }
}
