using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoading : MonoBehaviour {

  private Vector3 previousTerrainSpawnLoc;

	// Use this for initialization
	void Start () {
    previousTerrainSpawnLoc = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
    if(transform.position.x > previousTerrainSpawnLoc.x + 100)
    {

      Debug.Log("Location: " + previousTerrainSpawnLoc.x);
      // Spawn a new terrain piece
      /* some code goes here */
      // Chose a random terrain piece
      // Spawn it at the edge of the last terrain piece

      // Then save the most recent spawned terrain
      previousTerrainSpawnLoc = transform.position;
    }

	}
}
