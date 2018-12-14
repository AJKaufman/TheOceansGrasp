using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    GameObject win;
    GameObject player;
    GameObject escape;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sub")
        {
            Destroy(gameObject);
            player.SetActive(false);
            win.SetActive(true);
            escape.SetActive(true);
        }
    }
}