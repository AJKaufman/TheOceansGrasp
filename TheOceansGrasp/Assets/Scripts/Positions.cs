using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positions : MonoBehaviour {

    public static Positions instance = null;
    public List<Vector3> positions = new List<Vector3>();
    public int whatTile = 0;
    public bool outside = false;
    public List<GameObject> damagedNodes = new List<GameObject>();
    public List<GameObject> damagedCameras = new List<GameObject>();
    public Transform universalParent;
    public GameObject lose;
    public GameObject player;
    public AudioClip[] songs = new AudioClip[3];
    public bool tooClose = false;
    private Transform shark;
    private Transform sub;
    private AudioSource jukebox;
    public Slider energySlider;
    public GameObject frontScreen;
    public GameObject leftScreen;
    public GameObject rightScreen;
    public GameObject rearScreen;
    public GameObject buttonManager;
    public GameObject escape;

    public bool firstFrame = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        jukebox = gameObject.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        jukebox.loop = true;
        jukebox.clip = songs[0];
        jukebox.Play();
        sub = FindObjectOfType<SubmarineMovement>().gameObject.transform;
        shark = FindObjectOfType<BaskingShark>().gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {

        // regen energy when the player is outside
        if(outside)
        {
            sub.GetComponent<SubVariables>().gainEnergy();
        }

        // prevent the submarine moving if the energy hits 0
        if(energySlider.value <= 0 && firstFrame)
        {
            firstFrame = false;
            // turn off turbo dash
            if (sub.GetComponent<SubmarineMovement>().boosting)
            {
                buttonManager.GetComponent<DischargePrompt>().ToggleTurbo();
            }

            // shuts down the enhanced screens
            frontScreen.GetComponent<CameraFPS>().highfps = false;
            rearScreen.GetComponent<CameraFPS>().highfps = false;
            leftScreen.GetComponent<CameraFPS>().highfps = false;
            rightScreen.GetComponent<CameraFPS>().highfps = false;

            // turn off lights
            if (frontScreen.GetComponent<LightsOn>().on)
            {
                frontScreen.GetComponent<LightsOn>().TurnOn();
            }
            if (rightScreen.GetComponent<LightsOn>().on)
            {
                rightScreen.GetComponent<LightsOn>().TurnOn();
            }
            if (leftScreen.GetComponent<LightsOn>().on)
            {
                leftScreen.GetComponent<LightsOn>().TurnOn();
            }
            if (rearScreen.GetComponent<LightsOn>().on)
            {
                rearScreen.GetComponent<LightsOn>().TurnOn();
            }

            // sub movement is disabled
            sub.GetComponent<SubmarineMovement>().enabled = false;
            sub.GetComponent<Rigidbody>().isKinematic = true;
            Invoke("TurnBackOn", 8.0f);
        }


        float howClose = 0;
        if (outside)
        {
            howClose = player.transform.position.z - shark.position.z;
        }
        else
        {
            howClose = sub.transform.position.z - shark.position.z;
        }
        if (tooClose)
        {
            if (howClose > 250.0f)
            {
                tooClose = false;
                if (outside)
                {
                    ChangeMusic(2);
                }
                else
                {
                    ChangeMusic(0);
                }
            }
        }
        else
        {
            if (howClose < 200.0f)
            {
                tooClose = true;
                ChangeMusic(1);
            }
        }
	}

    public void Lose()
    {
        lose.SetActive(true);
        player.SetActive(false);
        escape.SetActive(true);
    }

    public void ChangeMusic(int whatSong)
    {
        jukebox.clip = songs[whatSong];
        jukebox.Play();
    }

    public void TurnBackOn()
    {
        firstFrame = true;
        sub.GetComponent<SubVariables>().gainEnergy(30.0f);
        sub.GetComponent<Rigidbody>().isKinematic = false;
        sub.GetComponent<SubmarineMovement>().enabled = true;
    }
}
