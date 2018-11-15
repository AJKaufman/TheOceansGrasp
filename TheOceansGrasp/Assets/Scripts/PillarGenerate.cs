using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGenerate : MonoBehaviour {
    int whatTile = 0;
    Random myRand;
    // Use this for initialization
	void Start () {
        myRand = new Random();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PillarGeneration()
    {
        int howMany = Random.Range(3, 5);
        float min = whatTile * 200;
        float max = whatTile * 200 + 200;
        float div = 200.0f / howMany;
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < howMany; i++)
        {
            int mult = Random.Range(1, 2);
            for (int j = 0; j < mult; j++)
            {
                int cluster = Random.Range(0, 1);
                float howFar = Random.Range(())
            }
            
        }
    }
}
