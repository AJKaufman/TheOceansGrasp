﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHitbox : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), GetComponentInParent<CapsuleCollider>());
        Physics.IgnoreCollision(gameObject.GetComponentInParent<CapsuleCollider>(), gameObject.GetComponent<CapsuleCollider>());
        Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), GetComponentInParent<BoxCollider>());
        Physics.IgnoreCollision(gameObject.GetComponentInParent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>());
    }

    // Update is called once per frame
    void Update () {
		
	}
}
