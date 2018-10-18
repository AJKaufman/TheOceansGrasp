using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaskingShark : MonoBehaviour {

    public float speed = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
                Destroy(fish.gameObject);
            }
        }
    }
}
