using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatFish : SeekerFish {

    [Header("Flat Fish")]
    public float cameraAttackRange = 2;
    public float intoHoverSpeed = 2;

    public float farAboveCamera = 2;
    public float closeAboveCamera = 0.2f;
    public float ontoCameraSpeed = 1;
    public float flattenOnCameraRate = 30; // In degrees

    // Should these all be audio sources?
    [Header("Audio")]
    public AudioClip randomSwimmingAudio;
    public AudioClip inAttackRangeAudio;
    public AudioClip attackAudio;
    public AudioClip fleeAudio;
    private AudioSource audioPlayer;

    private GameObject sub;
    private bool isCamera = false;
    private enum CameraAttackBehavior
    {
        Seek, Above, Attach, Attack
    };
    private CameraAttackBehavior camBehavior = CameraAttackBehavior.Seek;

    [Header("Timers")]
    public float attackDelay = 5;
    private float attackTimer;
    public float randomAudioTime;
    public float randomTimeAdded;
    private float randomAudioTimer;

    // Use this for initialization
    override protected void Start () {
        base.Start();

        audioPlayer = GetComponent<AudioSource>();
        attackTimer = attackDelay;
        randomAudioTimer = randomAudioTime + Random.Range(0, randomTimeAdded);
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    protected override void WanderBehavior()
    {
        base.WanderBehavior();
        randomAudioTimer -= Time.deltaTime;
        if (randomAudioTimer < 0)
        {
            audioPlayer.clip = randomSwimmingAudio;
            audioPlayer.Play();
            randomAudioTimer = randomAudioTime + Random.Range(0, randomTimeAdded);
        }
    }

    override protected void SeekBehavior()
    {
        if (!targetObject)
        {
            behaviour = FishBehaviour.Wander;
            return;
        }

        if (targetObject.tag == "Sub")
        {
            sub = targetObject;
            targetObject = GetNearestCamera(sub);
            if (!targetObject)
            {
                behaviour = FishBehaviour.Wander;
            }
            else
            {
                targetPosition = targetObject.transform.position;
            }
        }
        else if(targetObject.tag == "Player")
        {
            Flee(targetObject);
            audioPlayer.clip = fleeAudio;
            audioPlayer.Play();
            return;
        }

        if (isCamera)
        {
            switch (camBehavior)
            {
                case CameraAttackBehavior.Seek:
                    if (Vector3.SqrMagnitude(targetPosition - transform.position) <= cameraAttackRange * cameraAttackRange)
                    {
                        audioPlayer.clip = inAttackRangeAudio;
                        audioPlayer.Play();
                        camBehavior = CameraAttackBehavior.Above;
                        print("into above");
                    }
                    break;

                case CameraAttackBehavior.Above:
                    targetPosition = (targetObject.transform.forward * farAboveCamera) + targetObject.transform.position;
                    if (Vector3.SqrMagnitude(targetPosition - transform.position) <= stopDistance * stopDistance)
                    {
                        camBehavior = CameraAttackBehavior.Attach;
                        print("into attach");
                    }
                    break;

                case CameraAttackBehavior.Attach:
                    targetPosition = (targetObject.transform.forward * closeAboveCamera) + targetObject.transform.position;
                    if (Vector3.SqrMagnitude(targetPosition - transform.position) <= stopDistance * stopDistance)
                    {
                        camBehavior = CameraAttackBehavior.Attack;
                    }
                    break;

                case CameraAttackBehavior.Attack:
                    targetPosition = (targetObject.transform.forward * closeAboveCamera) + targetObject.transform.position;
                    attackTimer -= Time.deltaTime;
                    if (attackTimer < 0)
                    {
                        Attack();
                        attackTimer = attackDelay;
                    }
                    break;

                default:
                    print("Unrecognized FlatFish behavior");
                    break;
            }
        }
        else
        {
            base.SeekBehavior();
        }
    }

    // Return the nearest camera that is a child of the sub
    private GameObject GetNearestCamera(GameObject sub)
    {
        GameObject camera = null;

        float distance = float.MaxValue;
        Camera[] cameras = sub.GetComponentsInChildren<Camera>();
        foreach (Camera c in cameras)
        {
            float newDist = Vector3.SqrMagnitude(c.transform.position - transform.position);
            if (newDist < distance)
            {
                CameraFPS cameraFPS = GetCameraFPS(c);
                if (cameraFPS && !cameraFPS.targeted)
                {
                    distance = newDist;
                    if (camera)
                    {
                        GetCameraFPS(camera.GetComponent<Camera>()).targeted = false;
                    }
                    cameraFPS.targeted = true;
                    camera = c.gameObject;
                    isCamera = true;
                }
            }
        }

        if (!camera)
        {
            Flee(sub);
        }

        return camera;
    }

    // Return the next further camera that is a child of the sub
    private GameObject GetNextCamera(GameObject sub, GameObject currentCam)
    {
        GameObject camera = null;

        bool nextCamsTargeted = false;
        float dotValue = float.MaxValue;
        float currentDot = Vector3.Dot(sub.transform.forward, currentCam.transform.position);
        Camera[] cameras = sub.GetComponentsInChildren<Camera>();
        foreach (Camera c in cameras)
        {
            float newDot = Vector3.Dot(sub.transform.forward, c.transform.position);
            if (newDot > currentDot + 1 && newDot < dotValue)
            {
                CameraFPS fps = GetCameraFPS(c);
                if (fps && !fps.targeted)
                {
                    dotValue = newDot;
                    if (camera)
                    {
                        GetCameraFPS(camera.GetComponent<Camera>()).targeted = false;
                    }
                    else
                    {
                        GetCameraFPS(currentCam.GetComponent<Camera>()).targeted = false;
                    }
                    fps.targeted = true;
                    camera = c.gameObject;
                    nextCamsTargeted = false;
                }
                else
                {
                    nextCamsTargeted = true;
                }
            }
        }

        if (nextCamsTargeted)
        {
            Flee(sub);
            print("next cam is targeted");
        }

        return camera;
    }

    private void Attack()
    {
        if(isCamera && targetObject.GetComponent<Camera>())
        {
            CameraFPS cameraFPS = GetCameraFPS(targetObject.GetComponent<Camera>());
                if (cameraFPS.renderCam)
                {
                    audioPlayer.clip = attackAudio;
                    audioPlayer.Play();
                    cameraFPS.Damage();
                    print(name + " attacked");

                    GameObject nextCam = GetNextCamera(sub, targetObject);
                    if(nextCam && nextCam != targetObject)
                    {
                        targetObject = nextCam;
                        camBehavior = CameraAttackBehavior.Seek;
                    }
                    else
                    {
                        // Temp
                        //Flee(sub);
                    }
                    return;
                }
        }

        print(name + " unable to attack.");
    }

    private CameraFPS GetCameraFPS(Camera cam)
    {
        CameraFPS fps = null;

        CameraFPS[] cameraCanvases = FindObjectsOfType<CameraFPS>();
        foreach (CameraFPS c in cameraCanvases)
        {
            if (c.renderCam == cam)
            {
                fps = c;
                break;
            }
        }

        return fps;
    }

    protected override void Move()
    {
        if (isCamera)
        {
            switch (camBehavior)
            {
                case CameraAttackBehavior.Seek:
                    base.Move();
                    break;

                case CameraAttackBehavior.Above:
                    Vector3 rotation = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, Mathf.Deg2Rad * maxSpeedTurnRate * Time.deltaTime, 1);
                    Quaternion prevRotation = transform.rotation;
                    transform.rotation = Quaternion.LookRotation(rotation);

                    Velocity = (targetPosition - transform.position).normalized * intoHoverSpeed;
                    break;

                case CameraAttackBehavior.Attach:
                    Vector3 rot = Vector3.RotateTowards(transform.up, targetObject.transform.forward, flattenOnCameraRate * Mathf.Deg2Rad * Time.deltaTime, 1);
                    transform.up = rot;
                    Velocity = (targetPosition - transform.position).normalized * ontoCameraSpeed;
                    break;

                case CameraAttackBehavior.Attack:
                    Velocity = Vector3.zero;
                    transform.position = targetPosition;
                    transform.up = targetObject.transform.forward;
                    break;

                default: print("Unrecognized FlatFish behavior");
                    break;
            }

            rb.angularVelocity = Vector3.zero;
            rb.velocity = Velocity;
        }
        else
        {
            base.Move();
        }
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        // Do nothing
    }

    public override void Flee(GameObject fleeFrom)
    {
        Camera cam = targetObject.GetComponent<Camera>();
        if (cam)
        {
            GetCameraFPS(cam).targeted = false;
        }
        base.Flee(fleeFrom);
        isCamera = false;
    }

    protected override void SetSeekTarget(GameObject seekTarget)
    {
        if (seekTarget.tag == "Player")
        {
            Camera cam = targetObject.GetComponent<Camera>();
            if (cam)
            {
                CameraFPS fps = GetCameraFPS(cam);
                fps.targeted = false;
            }
        }
        base.SetSeekTarget(seekTarget);
    }

    protected override bool IsTarget(string tag)
    {
        if (tag == "Sub")
        {
            return false;
        }
        else
        {
            return base.IsTarget(tag);
        }
    }

    public bool IsAttached()
    {
        return camBehavior == CameraAttackBehavior.Attack;
    }
}
