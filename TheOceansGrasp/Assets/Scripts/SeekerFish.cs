using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeekerFish : MonoBehaviour {
    // NOTE: Much of this will be moved to a base fish class later

    public float maxSpeed;
    public float acceleration;
    public float maxTurnRate; // In degrees per second
    public float stopDistance = 0.5f; // Distance at which the object will stop from the target

    public float damage = 1;

    public bool willArrive = false;
    public float arriveDistance; // Distance from target that the object will start slowing down
    public float minArriveSpeed; // Minimum speed allowed when arriving

    public float wanderAngle = 180; // Angle from straight to get new point
    public float minWanderDistance = 5;
    public float maxWanderDistance = 20;

    // Public for testing
    public float stunTime = 0;

    public int aggroRange = 20;

    public Vector3 Velocity;// { get; private set; }
    private float speed = 0;
    // Public for testing
    public Vector3 targetPosition;
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
    public float maxSeekDistance = 100;


    // Use this for initialization
    void Start () {
        speed = maxSpeed;
        Velocity = Vector3.forward * speed;
        rb = GetComponent<Rigidbody>();
        targetPosition = GetRandomWanderDestination();
        behaviour = FishBehaviour.Wander;
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
                WanderBehavior();
                break;

            case FishBehaviour.Seek:
                CheckAggroRange();
                SeekBehavior();
                break;

            case FishBehaviour.Flee:
                FleeBehavior();
                break;

            default:
                Debug.LogError(gameObject.name + " has an invalid fish behavior.");
                break;
        }

        // Rotate the object to face its target
        Vector3 rotation = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, Mathf.Deg2Rad * maxTurnRate * Time.deltaTime, 1);
        Quaternion prevRotation = transform.rotation;
        transform.rotation = Quaternion.LookRotation(rotation);

        //Avoidance kinda
        float tempFishRadius = 0.5f;
        int framesAhead = 30; // Too far?
        //float tempHalfFishLength = 1;
        //RaycastHit[] rayDatas = Physics.SphereCastAll(transform.position + (transform.forward * tempHalfFishLength), tempFishRadius, transform.forward, speed * Time.deltaTime * framesAhead); // Use current speed rather than maxSpeed
        Debug.DrawRay(transform.position, transform.forward * speed * Time.deltaTime * framesAhead, Color.red);
        RaycastHit rayData = new RaycastHit();
        if (Physics.SphereCast(transform.position, tempFishRadius, transform.forward, out rayData, speed * Time.deltaTime * framesAhead))
        //if (rayDatas.Length > 0)
        {
            //foreach (RaycastHit r in rayDatas)
            { 
                // We hit an obstacle, now see if it actually is an obstacle instead of a fish or sub (sub will be interesting as it is both target and obstacle)
                // Get obstacle tag list?
                if (!IsTarget(rayData.collider.tag))
                {

                    //Edit this more. This is temporary
                    /*
                    //Gets stuck if head on
                    //Use dot product to tell and exaggerate the  difference?
                    transform.rotation = prevRotation;
                    Vector3 newDestination = rayData.point + (rayData.normal * (rayData.distance + tempFishRadius));
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, newDestination - transform.position, Mathf.Deg2Rad * maxTurnRate * Time.deltaTime, 1));
                    //Compare normal to transform.forward after(?) first obstacle found to determine distance along normal to go to
                    //Should this follow max turn rate? I think yes. Which means undo the turn and redo towards this instead. What happens if still hitting obstacle as the new destination is too far to turn?
                    //Could slow down if this happens to allow for more turn?
                    //speed -= 2 * acceleration * Time.deltaTime; // Slow down, later will add 1 acceleration
                    /**/
                    //Options:
                    // 1) redo turn vector
                    // 2) adjust current turn vector (issues with maxTurnRate?)
                    // 3) remove maxTurnRate and use flocker things only
                    //break;
                }
            }
        }

        float magnitude = Vector3.Magnitude(targetPosition - transform.position);
        if (magnitude <= stopDistance)
        {
            if (behaviour == FishBehaviour.Wander)
            {
                targetPosition = GetRandomWanderDestination();
            }
            else
            {
                Velocity = Vector3.zero;
            }
        }
        else
        {
            speed = Mathf.Min(maxSpeed, speed + (acceleration * Time.deltaTime));
            Velocity = transform.forward * speed;
            if (willArrive && arriveDistance > 0)
            {
                if (magnitude <= arriveDistance)
                {
                    speed = Mathf.Max(magnitude / arriveDistance, minArriveSpeed);
                    Velocity *= speed;
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
            if (Physics.Raycast(transform.position, g.transform.position - transform.position, out rayData, aggroRange) && rayData.collider.gameObject == g) {
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

    bool IsTarget(string tag)
    {
        foreach (SeekPriorities s in tagPriorities)
        {
            if(s.tag == tag)
            {
                return true;
            }
        }

        return false;
    }

    public void Flee(GameObject fleeFrom)
    {
        targetObject = fleeFrom;
        behaviour = FishBehaviour.Flee;
    }

    protected void WanderBehavior()
    {
        targetObject = null;
        seekPriority = int.MaxValue;
    }

    protected void SeekBehavior()
    {
        if (targetObject && Vector3.Magnitude(targetObject.transform.position - transform.position) < maxSeekDistance)
        {
            // TODO: Get target velocity if it is the player, sub, or fish
            targetPosition = targetObject.transform.position;
        }
        else
        {
            behaviour = FishBehaviour.Wander;
            targetPosition = GetRandomWanderDestination();
        }
    }

    protected void FleeBehavior()
    {
        Vector3 away = transform.forward;
        if (targetObject)
        {
            if (Vector3.Magnitude(targetObject.transform.position - transform.position) > maxSeekDistance)
            {
                // Remove fish after certain distance away from flee target
                Destroy(gameObject);
            }
            else
            {
                away = transform.position - targetObject.transform.position;
            }
        }
        targetPosition = transform.position + (away.normalized * 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kamikaze fish - destroy on contact with a target object, not necessarily the one that is being sought 
        if(IsTarget(collision.gameObject.tag))
        {
            SubVariables subVar = collision.gameObject.GetComponent<SubVariables>();
            if (subVar){
                subVar.loseHealth(damage);
            }
            // Do similar for player

            Destroy(gameObject);
        }
    }
}
