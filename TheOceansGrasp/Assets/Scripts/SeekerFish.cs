using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeekerFish : MonoBehaviour {
    // NOTE: Much of this will be moved to a base fish class later

    public float maxSpeed;
    public float acceleration;
    public float maxSpeedTurnRate; // In degrees per second
    public float stoppedTurnRate;// In degrees per second
    public float stopDistance = 0.5f; // Distance at which the object will stop from the target

    public float damage = 1;

    public bool willArrive = false;
    public float arriveDistance; // Distance from target that the object will start slowing down
    public float minArriveSpeed; // Minimum speed allowed when arriving

    public float wanderAngle = 180; // Angle from straight to get new point
    public float minWanderDistance = 5;
    public float maxWanderDistance = 20;
    public float wanderTargetTime = 5; // How long to seek the wander target
    //private float wanderTimer; // When zero, find new target
    public float wanderDeviation = 2;
    private Vector3 prevWanderDir = Vector3.zero;
    public float wanderDistance = 0.5f; // The larger this is, the more the deviation will affect wander
    public float prevWanderWeight = 8;

    private float stunTimer = 0;

    public float lifetime = 120; // Time in seconds for the fish to last when wandering, does not reset

    // Max range to check for targets (length of the raycast)
    protected float maxAggroRange = 0;

    public Vector3 Velocity;// { get; private set; }
    protected float speed = 0;
    public float avoidanceScale = 1;
    // Public for testing
    public Vector3 targetPosition;
    public GameObject targetObject; //Need to know what to search for first
    protected Rigidbody rb;

    protected enum FishBehaviour
    {
        Wander, Seek, Flee
    };
    protected FishBehaviour behaviour = FishBehaviour.Wander;

    [System.Serializable]
    public class SeekPriorities
    {
        public string tag;
        public int priority;
        public float aggroRange;
    }
    public SeekPriorities[] tagPriorities;
    // Public for testing
    public int seekPriority = int.MaxValue;


    // Use this for initialization
    virtual protected void Start () {
        speed = 0;
        Velocity = Vector3.forward * speed;
        rb = GetComponent<Rigidbody>();
        targetPosition = GetRandomWanderDestination();
        behaviour = FishBehaviour.Wander;
        //wanderTimer = wanderTargetTime;
        prevWanderDir = transform.forward * prevWanderWeight;

        foreach (SeekPriorities s in tagPriorities)
        {
            if (s.aggroRange > maxAggroRange)
            {
                maxAggroRange = s.aggroRange;
            }
        }

        Physics.IgnoreCollision(FindObjectOfType<SubVariables>().GetComponent<BoxCollider>(), GetComponentInChildren<Collider>());
    }
	
	// Update is called once per frame
	virtual protected void Update () {
        if(stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
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

        //Move();
	}

    protected void FixedUpdate()
    {
        if(stunTimer <= 0)
        {
            Move(maxSpeed);
        }
    }

    protected Vector3 GetRandomWanderDestination()
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
            if (Physics.Raycast(transform.position, (g.transform.position - transform.position).normalized, out rayData, maxAggroRange) && rayData.collider.gameObject == g) {
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
            if (IsGreaterPriorityInRange(pair.Key.tag, pair.Value))
            {
                SetSeekTarget(pair.Key);
            }
        }
    }

    bool IsGreaterPriorityInRange(string tag, float distance)
    {
        foreach (SeekPriorities s in tagPriorities)
        {
            if (tag == s.tag && s.priority < seekPriority && distance <= s.aggroRange)
            {
                seekPriority = s.priority;
                return true;
            }
        }

        return false;
    }

    virtual protected void SetSeekTarget(GameObject seekTarget)
    {
        behaviour = FishBehaviour.Seek;
        targetObject = seekTarget;
    }

    virtual protected bool IsTarget(string tag)
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

    virtual public void Flee(GameObject fleeFrom)
    {
        targetObject = fleeFrom;
        behaviour = FishBehaviour.Flee;
    }

    virtual protected void WanderBehavior()
    {
        /*
        wanderTimer -= Time.deltaTime;
        if(wanderTimer < 0)
        {
            targetPosition = GetRandomWanderDestination();
            wanderTimer = wanderTargetTime;
        }
        /**/
        targetObject = null;
        seekPriority = int.MaxValue;

        Vector3 newDir = new Vector3(Random.Range(-wanderDeviation, wanderDeviation), Random.Range(-wanderDeviation, wanderDeviation), Random.Range(-wanderDeviation, wanderDeviation)).normalized * wanderDistance;
        targetPosition = transform.position + prevWanderDir + newDir;
        prevWanderDir = (prevWanderDir + newDir).normalized * prevWanderWeight;

        lifetime -= Time.deltaTime;
        if(lifetime < 0)
        {
            // This may crash
            Flee(FindObjectOfType<SubmarineMovement>().gameObject);
        }
    }

    virtual protected void SeekBehavior()
    {
        if (targetObject && Vector3.Magnitude(targetObject.transform.position - transform.position) < maxAggroRange)
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

    virtual protected void FleeBehavior()
    {
        Vector3 away = transform.forward;
        if (targetObject)
        {
            if (Vector3.Magnitude(targetObject.transform.position - transform.position) > maxAggroRange)
            {
                // Remove fish after certain distance away from flee target
                Kill();
            }
            else
            {
                away = transform.position - targetObject.transform.position;
            }
        }
        else
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 0)
            {
                Kill();
            }
        }
        targetPosition = transform.position + (away.normalized * 10);
    }

    virtual protected void Move(float currentMaxSpeed, bool useRigidBody = true)
    {
        //Avoidance kinda
        float tempFishRadius = 0.5f;
        float halfFishLength = 1;
        int framesAhead = 90; // Too far?
        // Make sure the start of the check is in the fish so that it will not be inside the obstacle
        Debug.DrawRay(transform.position - (transform.forward * halfFishLength), transform.forward * speed * Time.deltaTime * framesAhead, Color.red);
        RaycastHit rayData = new RaycastHit();
        if (Physics.SphereCast(transform.position - (transform.forward * halfFishLength), tempFishRadius, transform.forward, out rayData, speed * Time.deltaTime * framesAhead))
        {
            // We hit an obstacle, now see if it actually is an obstacle instead of a fish or sub (sub will be interesting as it is both target and obstacle)
            // Get obstacle tag list?
            if (!IsTarget(rayData.collider.tag))
            {
                /*
                speed -= acceleration * Time.deltaTime; // Slow down
                speed = Mathf.Max(speed, 0);
                /**/
                float right = Vector3.Dot(rayData.point - transform.position, transform.right * tempFishRadius);
                float up = Vector3.Dot(rayData.point - transform.position, transform.up * tempFishRadius);
                Vector3 newDir = (transform.right * (1-right) * avoidanceScale) + (transform.up * (1-up) * avoidanceScale) + (transform.forward * avoidanceScale);
                //targetPosition = newDir + transform.position;
                // Rotate the object to face its target
                Vector3 rotation = Vector3.RotateTowards(transform.forward, newDir, Mathf.Deg2Rad * maxSpeedTurnRate * Time.deltaTime, 1);
                //Quaternion prevRotation = transform.rotation;
                transform.rotation = Quaternion.LookRotation(rotation);
                //transform.rotation = Quaternion.LookRotation(newDir);
                /*
                transform.rotation = prevRotation;
                Vector3 newDestination = rayData.point + (rayData.normal * (rayData.distance + tempFishRadius));
                float turnRate = Mathf.Deg2Rad * Mathf.Max(maxSpeedTurnRate, stoppedTurnRate * (maxSpeed / (speed + maxSpeed))) * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, newDestination - transform.position, Mathf.Deg2Rad * turnRate * Time.deltaTime, 1));
                speed -= acceleration * Time.deltaTime; // Slow down, later will add 1 acceleration
                /**/
            }
        }
        else
        {
            // Rotate the object to face its target
            Vector3 rotation = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, Mathf.Deg2Rad * maxSpeedTurnRate * Time.deltaTime, 1);
            //Quaternion prevRotation = transform.rotation;
            transform.rotation = Quaternion.LookRotation(rotation);
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
            speed = Mathf.Min(currentMaxSpeed, speed + (acceleration * Time.deltaTime));
            Velocity = transform.forward * speed;
            if (willArrive && arriveDistance > 0)
            {
                if (magnitude <= arriveDistance)
                {
                    Velocity /= speed;
                    speed = Mathf.Max(magnitude / arriveDistance, minArriveSpeed);
                    Velocity *= speed;
                }
            }
        }
        //rb.velocity = Velocity;
        if (useRigidBody)
        {
            //rb.MovePosition((Velocity * Time.deltaTime) + transform.position);
            Vector3 vChange = Velocity - rb.velocity;
            vChange = Vector3.ClampMagnitude(vChange, acceleration * Time.deltaTime);
            rb.AddForce(vChange, ForceMode.VelocityChange);
        }
        else
        {
            transform.position += Velocity * Time.deltaTime;
        }
    }

    public void Stun(float time)
    {
        stunTimer = time;
    }

    public void Stun(float time, GameObject fleeFrom)
    {
        stunTimer = time;
        Flee(fleeFrom);
    }

    virtual protected void OnCollisionEnter(Collision collision)
    {
        // Kamikaze fish - destroy on contact with a target object, not necessarily the one that is being sought 
        if(IsTarget(collision.gameObject.tag))
        {
            SubVariables subVar = collision.gameObject.GetComponent<SubVariables>();
            if (subVar){
                subVar.loseHealth(damage);
            }
            // Do similar for player

            Kill();
        }
    }

    virtual public void Kill()
    {
        Destroy(gameObject);
    }
}
