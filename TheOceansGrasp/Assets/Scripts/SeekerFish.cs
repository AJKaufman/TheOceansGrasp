using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeekerFish : MonoBehaviour {
    // NOTE: Much of this will be moved to a base fish class later

    public float maxSpeed;
    public float maxTurnRate; // In degrees per second
    public float stopDistance = 0.5f; // Distance at which the object will stop from the target
    public bool willArrive = false;
    public float arriveDistance; // Distance from target that the object will start slowing down
    public float minArriveSpeed; // Minimum speed allowed when arriving

    public float wanderAngle = 180; // Angle from straight to get new point
    public float minWanderDistance = 5;
    public float maxWanderDistance = 20;

    // Public for testing
    public float stunTime = 0;

    public int aggroRange = 20;

    public Vector3 Velocity { get; private set; }
    // Public for testing
    public Vector3 targetPosition;

    private enum TargetType
    {
        None, Player, Sub, Fish
    };
    private TargetType targetType;
    // Public for testing
    public GameObject targetObject; //Need to know what to search for first
    private Rigidbody rb;

    private enum FishBehaviour
    {
        Wander, Seek, Flee
    };
    private FishBehaviour behaviour = FishBehaviour.Wander;

    [System.Serializable]
    public class SeekPriorities
    {
        public string tag;
        public int priority;
    }
    public SeekPriorities[] tagPriorities;
    // Public for testing
    public int seekPriority = int.MaxValue;


    // Use this for initialization
    void Start () {
        Velocity = Vector3.forward * maxSpeed;
        rb = GetComponent<Rigidbody>();
        targetPosition = GetRandomWanderDestination();
    }
	
	// Update is called once per frame
	void Update () {
        //For debug only
        if (Input.GetKeyDown("u"))
        {
            switch (behaviour)
            {
                case FishBehaviour.Wander:
                case FishBehaviour.Seek:
                    behaviour = FishBehaviour.Flee;
                    break;

                case FishBehaviour.Flee:
                    Debug.Log("Fleeing");
                    behaviour = FishBehaviour.Wander;
                    break;
            }
        }

        if(stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }

        switch (behaviour)
        {
            case FishBehaviour.Wander:
                CheckAggroRange();
                break;

            case FishBehaviour.Seek:
                CheckAggroRange();

                if (targetObject)
                {
                    // TODO: Get target velocity if it is the player, sub, or fish
                    targetPosition = targetObject.transform.position;
                }
                break;

            case FishBehaviour.Flee:
                Vector3 away = transform.forward;
                if (targetObject)
                {
                    away = transform.position - targetObject.transform.position;
                }
                targetPosition = transform.position + (away.normalized * 10);
                break;

            default:
                Debug.LogError(gameObject.name + " has an invalid fish behavior.");
                break;
        }

        /*
        CheckAggroRange();

        if (targetObject)
        {
            // TODO: Get target velocity if it is the player, sub, or fish
            targetPosition = targetObject.transform.position;
        }
        */
        // Rotate the object to face its target
        Vector3 rotation = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, Mathf.Deg2Rad * maxTurnRate * Time.deltaTime, 1);
        transform.rotation = Quaternion.LookRotation(rotation);

        //Check for obstacles?
        float tempFishRadius = 0.5f;
        int framesAhead = 5; // Too far?
        // May be issue with this if it hits something 
        RaycastHit[] rayDatas = Physics.SphereCastAll(transform.position, tempFishRadius, transform.forward, maxSpeed * Time.deltaTime * framesAhead); // Use current speed rather than maxSpeed
        if(rayDatas.Length > 0)
        {
            // We hit an obstacle, now see if it actually is an obstacle instead of a fish or sub (sub will be interesting as it is both target and obstacle)
            // Get obstacle tag list?

            //Options:
                // 1) redo turn vector
                // 2) adjust current turn vector (issues with maxTurnRate?)
        }

        float magnitude = Vector3.Magnitude(targetPosition - transform.position);
        if (magnitude <= stopDistance)
        {
            if (behaviour == FishBehaviour.Wander)
            {
                targetObject = null;
                targetPosition = GetRandomWanderDestination();
            }
            else
            {
                Velocity = Vector3.zero;
            }
        }
        else
        {
            Velocity = transform.forward * maxSpeed;
            if (willArrive && arriveDistance > 0)
            {
                if (magnitude <= arriveDistance)
                {
                    Velocity *= Mathf.Max(magnitude / arriveDistance, minArriveSpeed);
                }
            }
        }
        rb.velocity = Velocity;
	}

    Vector3 GetRandomWanderDestination()
    {
        int sign = Random.Range(0, 2) - 1;
        if (sign == 0)
        {
            sign++;
        }

        float angleX = Random.Range(0, wanderAngle);
        float angleY = Random.Range(0, wanderAngle);
        float angleZ = Random.Range(0, wanderAngle);
        Vector3 angles = new Vector3(angleZ, angleX, angleY);

        transform.Rotate(angles);
        Vector3 wander = transform.forward;
        wander = wander.normalized * Random.Range(minWanderDistance, maxWanderDistance);
        wander += transform.position;
        transform.Rotate(-angles);

        return wander;
    }

    void CheckAggroRange()
    {
        // Place in game manager and simply add each object to list on their spawn and remove on their despawn?
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<KeyValuePair<GameObject, float>> inRange = new List<KeyValuePair<GameObject, float>>();
        RaycastHit rayData = new RaycastHit();
        foreach (GameObject g in allObjects)
        {
            if (Physics.Raycast(transform.position, g.transform.position - transform.position, out rayData, aggroRange)) {
                inRange.Add(new KeyValuePair<GameObject, float>(g, rayData.distance));
            }
        }
        CheckTargets(ref inRange);
    }

    void CheckTargets(ref List<KeyValuePair<GameObject, float>> possibleTargets)
    {
        // Check all possible targets
        // Player is higher than sub usually, need to be to flee from
        foreach (KeyValuePair<GameObject, float> pair in possibleTargets)
        {
            if (IsGreaterPriority(pair.Key.tag))
            {
                behaviour = FishBehaviour.Seek;
                targetObject = pair.Key;
            }
        }
    }

    bool IsGreaterPriority(string tag)
    {
        foreach (SeekPriorities s in tagPriorities)
        {
            if (tag == s.tag && s.priority < seekPriority)
            {
                seekPriority = s.priority;
                return true;
            }
        }

        return false;
    }
}
