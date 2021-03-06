﻿using System.Collections;
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

    public Vector3 forwardAxis = new Vector3(0, 0, 1);
    public Vector3 rightAxis = new Vector3(1, 0, 0);
    public Vector3 upAxis = new Vector3(0, 1, 0);
    public Vector3 middlePosition = new Vector3(0, 0, 0);

    public bool DogSpawned { get; set; }
    public float dogSpawnDistance = 100;
    public float checkTime;
    private float checkTimer;
    [Range(0,100)]
    public float spawnChancePerCheck = 0.1f;
    public GameObject dogPrefab;
    private BaskingShark shark;

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
            Vector3 spawnPosition = (instance.forwardAxis * spawnDistance) + (instance.rightAxis * spawnWidth) + (instance.upAxis * spawnHeight) + Vector3.Project(transform.position, instance.forwardAxis) + instance.middlePosition;
            GameObject fish = Instantiate(fishPrefab);
            fish.transform.position = spawnPosition;
            fish.transform.rotation = Quaternion.LookRotation(transform.position - fish.transform.position);
        }
    }
    public FishSpawn[] fishToSpawn;

	// Use this for initialization
	void Start () {
        instance = this;
        shark = FindObjectOfType<BaskingShark>();
        foreach (FishSpawn f in fishToSpawn)
        {
            Debug.Assert(f.fishPrefab);
            f.ResetTimer();
        }
    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        checkTimer -= dt;
        foreach (FishSpawn f in fishToSpawn)
        {
            f.Tick(dt, transform);
        }

        if(!DogSpawned && checkTimer < 0)
        {
            if((shark && (shark.transform.position - transform.position).sqrMagnitude > 4 * dogSpawnDistance * dogSpawnDistance) || !shark)
            {
                if (Random.Range(0, 100f) < spawnChancePerCheck)
                {
                    SpawnDog(dogPrefab);
                }
            }
            checkTimer = checkTime;
        }
	}

    void SpawnDog(GameObject prefab)
    {
        Vector3 start = new Vector3(0, 0, transform.position.z - (dogSpawnDistance - 10));
        Quaternion rot = new Quaternion();
        GameObject dog = Instantiate(prefab, start, rot);
        dog.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0));
    }
}
