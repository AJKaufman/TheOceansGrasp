using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerahover : MonoBehaviour {
    public GameObject Target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y+100f, Target.transform.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, -Target.transform.eulerAngles.y));
        //transform.forward = -Target.transform.up;
        //transform.up = Target.transform.forward;
    }
}
