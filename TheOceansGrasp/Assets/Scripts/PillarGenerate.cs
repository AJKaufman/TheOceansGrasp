using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGenerate : MonoBehaviour {
    public GameObject prefab;
    public List<GameObject> pillarList = new List<GameObject>();
    public float radius = 2.0f;
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PillarGeneration()
    {
        int howMany = Random.Range(2, 4);
        int whatRange = 0;
        float min = (Positions.instance.whatTile * 200) - 100;
        float max = (Positions.instance.whatTile * 200) + 100;
        float div = 200.0f / howMany;
        float howFar = 0;
        float howWide = 0;
        float whatAngle = 0;
        for (int i = 1; i <= howMany; i++)
        {
            int mult = Random.Range(1, 3);
            int[] really = { 0, 0, 0 };
            for (int j = 0; j < mult; j++)
            {
                bool mainValid = false;
                int cluster = Random.Range(0, 8);
                while (mainValid == false)
                {
                    howFar = Random.Range(min + (div * (i - 1)), (min + (div * i)));
                    whatRange = Random.Range(1, 4);
                    while (really[whatRange-1] > 0)
                    {
                        whatRange = Random.Range(1, 4);
                    }
                    if (whatRange == 3)
                    {
                        howWide = Random.Range(-50.0f, -17.0f);
                    }
                    if (whatRange == 2)
                    {
                        howWide = Random.Range(-17.0f, 17.0f);
                    }
                    else
                    {
                        howWide = Random.Range(-17.0f, 50.0f);
                    }
                    really[whatRange - 1]++;
                    whatAngle = Random.Range(0.0f, 359.0f);
                    Vector3 newVector = new Vector3(howWide, 0, howFar);
                    bool tempValid = true;
                    foreach (Vector3 pos in Positions.instance.positions)
                    {
                        if (Vector3.Distance(newVector, pos) < 10)
                        {
                            tempValid = false;
                        }
                    }
                    mainValid = tempValid;
                }
                GameObject newObject = (GameObject)Instantiate(prefab, new Vector3(howWide, 0, howFar),Quaternion.Euler(0,0,0));
                pillarList.Add(newObject);
                Positions.instance.positions.Add(newObject.transform.position);
                /*if (cluster == 1)
                {
                    Debug.Log("Cluster");
                    for (int clust = 0; clust < 3; clust++)
                    {
                        float x = 0, z = 0;
                        //while (cleared == false)
                    //{
                        whatAngle = Random.Range(0.0f, 359.0f);
                        int xz = Random.Range(0, 3);
                        int negPosx = Random.Range(0, 2);
                        int negPosz = Random.Range(0, 2);
                        if (xz == 2)
                        {
                            if (negPosx == 1)
                            {
                                x = howWide + Random.Range(12.0f, 20.0f);
                            }
                            else
                            {
                                x = howWide + Random.Range(-20.0f, -12.0f);
                            }
                            if (negPosz == 1)
                            {
                                z = howFar + Random.Range(12.0f, 20.0f);
                            }
                            else
                            {
                                z = howFar + Random.Range(-20.0f, -12.0f);
                            }
                        }
                        else if (xz == 1)
                        {
                            if (negPosx == 1)
                            {
                                x = howWide + Random.Range(0.0f, 12.0f);
                            }
                            else
                            {
                                x = howWide + Random.Range(-12.0f, 0.0f);
                            }
                            if (negPosz == 1)
                            {
                                z = howFar + Random.Range(12.0f, 20.0f);
                            }
                            else
                            {
                                z = howFar + Random.Range(-20.0f, -12.0f);
                            }
                        }
                        else
                        {
                            if (negPosx == 1)
                            {
                                x = howWide + Random.Range(12.0f, 20.0f);
                            }
                            else
                            {
                                x = howWide + Random.Range(-20.0f, -12.0f);
                            }
                            if (negPosz == 1)
                            {
                                z = howFar + Random.Range(0.0f, 12.0f);
                            }
                            else
                            {
                                z = howFar + Random.Range(-12.0f, 0.0f);
                            }
                        }
                        Vector3 newPos = new Vector3(x, 0, z);
                        GameObject subObject = (GameObject)Instantiate(prefab, new Vector3(x, 0, z), Quaternion.Euler(0, whatAngle, 0));
                        pillarList.Add(subObject);
                        Positions.instance.positions.Add(new Vector3(x, 0, z));
                    }
                }*/
            }
        }
        Positions.instance.whatTile++;
    }

    public void PillarDeletion()
    {
        for(int i = pillarList.Count-1; i >=0; i--)
        {
            Positions.instance.positions.Remove(pillarList[i].transform.position);
            Destroy(pillarList[i]);
        }
        pillarList.Clear();
    }
}
