using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{

  void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.tag == "Sub")
    {
      Destroy(gameObject);
      Application.Quit();
    }
  }
}