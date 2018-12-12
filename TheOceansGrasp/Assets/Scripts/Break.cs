using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour {
    public GameObject prefab1, prefab2, prefab3;
    public bool triggered = false;
    public bool player = false;
    public List<GameObject> chunkList = new List<GameObject>();
    public GameObject sub;
    public GameObject destroyer;
    int timer = 0;
    public bool destroyed = false;

    // Use this for initialization
    void Start () {
        sub = GameObject.FindGameObjectWithTag("Sub");
    }
	
	// Update is called once per frame
	void Update () {
        if(timer%10 == 5 && triggered == true && destroyed == false)
        {
            for(int i = 0; i < 3; i++)
            {
                chunkList[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, -.1f, 0));
            }
        }

        if (timer >= 500 && destroyed == false)
        {
            destroyed = true;
            chunkList.Clear();
        }
        timer++;

	}

    public void CreateChunks() {
        chunkList.Add(GameObject.Instantiate(prefab1, gameObject.transform.position+(sub.transform.forward*3), gameObject.transform.rotation));
        Vector3 transfer = gameObject.transform.position + (sub.transform.forward * 3);
        transfer.y += 20;
        chunkList.Add(GameObject.Instantiate(prefab2, transfer, gameObject.transform.rotation));
        transfer.y -= 40;
        chunkList.Add(GameObject.Instantiate(prefab3, transfer, gameObject.transform.rotation));
        
        for(int i = 0; i < 3; i++)
        {
            transfer = sub.transform.position;
            transfer.y -= 30;
            chunkList[i].GetComponent<Rigidbody>().AddForce(destroyer.transform.forward * 200.0f);
            chunkList[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-80.0f,80.0f), 0, Random.Range(-80.0f, 80.0f)));
            chunkList[i].GetComponent<Rigidbody>().AddTorque(Random.Range(-30.0f, 30.0f), Random.Range(-30.0f, 30.0f), Random.Range(-30.0f, 30.0f));
        }
        if (player == true)
        {
            sub.GetComponent<SubmarineMovement>().speed = 0;
            sub.GetComponent<Rigidbody>().isKinematic = true;
            sub.GetComponent<SubmarineMovement>().enabled = false;
            Invoke("TurnBackOn", 2.0f);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == sub.gameObject)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            triggered = true;
            player = true;
            sub.gameObject.GetComponent<SubVariables>().loseHealth(5.0f);
            destroyer = sub.gameObject;
            CreateChunks();
        }
        else if(collision.gameObject.tag == "PillarBreaker")
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            triggered = true;
            destroyer = collision.gameObject;
            CreateChunks();
        }
    }

    public void TurnBackOn()
    {
        sub.GetComponent<Rigidbody>().isKinematic = false;
        sub.GetComponent<SubmarineMovement>().enabled = true;
    }
}