using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaskingShark : MonoBehaviour {

    public float speed = 2;
    public float viewDistance = 100;

    public AudioSource constantAudio;
    public AudioSource roarAudio;
    public float baseRoarTime = 20;
    public float addRandomTime = 30;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = baseRoarTime + Random.Range(0, addRandomTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (CloseToPlayer())
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = baseRoarTime + Random.Range(0, addRandomTime);
                roarAudio.Play();
                constantAudio.Stop();
            }
            else if (!roarAudio.isPlaying && !constantAudio.isPlaying)
            {
                constantAudio.Play();
            }
        }
        else if(constantAudio.isPlaying)
        {
            constantAudio.Stop();
        }
	}

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        SubVariables health = other.GetComponent<SubVariables>();
        if (health)
        {
            // One hit kill
            health.loseHealth(int.MaxValue);
        }
        else
        {
            // Destroy fish
            //print("check for fish");
            SeekerFish fish = other.transform.root.GetComponent<SeekerFish>();
            if (fish)
            {
                //print("kill fish");
                fish.Kill();
            }
            else
            {
                PlayerSwim player = other.GetComponent<PlayerSwim>();
                if (player)
                {
                    FindObjectOfType<Positions>().Lose();
                }
            }
        }
    }

    private bool CloseToPlayer()
    {
        bool result = false;
        SubmarineMovement sub = FindObjectOfType<SubmarineMovement>();
        PlayerSwim player = FindObjectOfType<PlayerSwim>();

        if (sub)
        {
            if(Vector3.SqrMagnitude(transform.position - sub.transform.position) < viewDistance * viewDistance)
            {
                result = true;
            }
        }

        if (player)
        {
            if (Vector3.SqrMagnitude(transform.position - player.transform.position) < viewDistance * viewDistance)
            {
                result = true;
            }
        }

        return result;
    }
}
