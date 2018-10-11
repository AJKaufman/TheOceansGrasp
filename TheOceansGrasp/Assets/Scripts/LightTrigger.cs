using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

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
            FlatFish fish = other.GetComponent<FlatFish>();
            if (fish)
            {
                if (!fish.IsAttached())
                {
                    fish.Flee(gameObject);
                }
            }
        }
    }
}
