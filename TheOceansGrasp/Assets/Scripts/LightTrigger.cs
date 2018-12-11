using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public bool affectFlatFish = true;
    public bool alwaysHitFlatFish = false;//Needs affectFlatFish = true to work
    public bool affectDogFish = false;
    public bool affectSeekerFish = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Must be stay so that the light can be turned on and still affect fish
    private void OnTriggerStay(Collider other)
    {
        if (enabled)
        {
            if (affectFlatFish)
            {
                FlatFish fish = other.GetComponent<FlatFish>();
                if (fish && (alwaysHitFlatFish || !fish.IsAttached()))
                {
                    fish.Flee(gameObject);
                }
            }

            if (affectDogFish)
            {
                DogFish dog = other.GetComponent<DogFish>();
                if(dog)
                {
                    dog.SetInLight();
                }
            }

            if (affectSeekerFish)
            {
                SeekerFish2 seeker = other.GetComponent<SeekerFish2>();
                if(seeker)
                {
                    seeker.Flee(gameObject);
                }
            }
        }
    }
}
