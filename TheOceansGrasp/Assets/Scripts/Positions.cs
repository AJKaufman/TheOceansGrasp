using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (howClose > 150.0f)
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
            if (howClose < 115.0f)
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
    }

    public void ChangeMusic(int whatSong)
    {
        jukebox.clip = songs[whatSong];
        jukebox.Play();
    }
}
