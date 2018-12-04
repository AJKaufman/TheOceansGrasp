using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoading : MonoBehaviour {


    public GameObject terrain1;
    public GameObject terrain2;
    public GameObject terrain3;
    

    private int currentTerrain = 0;
    private Vector3 previousTerrainSpawnLoc = new Vector3(-100, -30, -200);

    // Use this for initialization
    void Start () {
        //terrain1.GetComponent<PillarGenerate>().PillarGeneration();
        Positions.instance.whatTile++;
        terrain2.GetComponent<PillarGenerate>().PillarGeneration();
        terrain3.GetComponent<PillarGenerate>().PillarGeneration();
    }
	
	// Update is called once per frame
	void Update () {
		
        // If you've passed the previous spawn area
        if(transform.position.z > previousTerrainSpawnLoc.z + 98 + 195)
        {

            // Spawn the next spawn area.

            Debug.Log("Location: " + previousTerrainSpawnLoc.z);

            // Choose which terrain to spawn next
            //currentTerrain = (int)Random.Range(1.0f, 4.0f);

            currentTerrain++;
            if (currentTerrain > 3) currentTerrain = 1;



            switch(currentTerrain)
            {
                case 1:
                    terrain1.transform.position = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 585);
                    terrain1.GetComponent<PillarGenerate>().PillarDeletion();
                    terrain1.GetComponent<PillarGenerate>().PillarGeneration();
                    previousTerrainSpawnLoc = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 195);
                    break;
                case 2:
                    terrain2.transform.position = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 585);
                    terrain2.GetComponent<PillarGenerate>().PillarDeletion();
                    terrain2.GetComponent<PillarGenerate>().PillarGeneration();
                    previousTerrainSpawnLoc = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 195);
                    break;
                case 3:
                    terrain3.transform.position = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 585);
                    terrain3.GetComponent<PillarGenerate>().PillarDeletion();
                    terrain3.GetComponent<PillarGenerate>().PillarGeneration();
                    previousTerrainSpawnLoc = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 195);
                    break;
                default:
                    terrain3.transform.position = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 585);
                    terrain3.GetComponent<PillarGenerate>().PillarDeletion();
                    terrain3.GetComponent<PillarGenerate>().PillarGeneration();
                    previousTerrainSpawnLoc = new Vector3(previousTerrainSpawnLoc.x, previousTerrainSpawnLoc.y, previousTerrainSpawnLoc.z + 195);
                    break;
            }
        }

	}
}
