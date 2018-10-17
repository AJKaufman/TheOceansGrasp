using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Place this script on the sub
 */
public class SubFishSpawner : MonoBehaviour {

    public static SubFishSpawner instance;
    public float minDistanceToSpawnAt = 20.0f;
    public float maxDistanceToSpawnAt = 60.0f;
    public float maxSideDistanceToSpawnAt = 30.0f;
    public float maxHeightToSpawnAt = 30.0f;

    [System.Serializable]
    public class FishSpawn
    {
        public GameObject fishPrefab;

        public float spawnTimer = 10.0f;
        public float timerDeviation = 2.0f; // Time in either direction that the timer may be modified
        private float timer = 0;
        private float spawnTime = 0;
        
        public void Tick(float deltaTime, Transform transform)
        {
            timer += deltaTime;
            if(timer > spawnTime)
            {
                SpawnAhead(ref transform);
                ResetTimer();
            }
        }

        public void ResetTimer()
        {
            float deviation = Random.Range(0, timerDeviation * 2) - timerDeviation;
            spawnTime = spawnTimer + deviation;
            timer = 0;
        }

        void SpawnAhead(ref Transform transform)
        {
            float spawnDistance = Random.Range(instance.minDistanceToSpawnAt, instance.maxDistanceToSpawnAt);
            float spawnWidth = Random.Range(0, instance.maxSideDistanceToSpawnAt * 2) - instance.maxSideDistanceToSpawnAt;
            float spawnHeight = Random.Range(0, instance.maxHeightToSpawnAt * 2) - instance.maxHeightToSpawnAt;
            Vector3 spawnPosition = (transform.forward * spawnDistance) + (transform.right * spawnWidth) + (transform.up * spawnHeight) + transform.position;
            GameObject fish = Instantiate(fishPrefab);
            fish.transform.position = spawnPosition;
            fish.transform.rotation = Quaternion.LookRotation(transform.position - fish.transform.position);
        }
    }
    public FishSpawn[] fishToSpawn;

	// Use this for initialization
	void Start () {
        instance = this;
        foreach (FishSpawn f in fishToSpawn)
        {
            Debug.Assert(f.fishPrefab);
            f.ResetTimer();
        }
    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        foreach (FishSpawn f in fishToSpawn)
        {
            f.Tick(dt, transform);
        }
	}
}
