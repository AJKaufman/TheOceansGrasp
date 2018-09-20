﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubVariables : MonoBehaviour {

  float health;
  float energy;
  public Slider displayedHealth;
  public Slider displayedEnergy;
    float radius;
    public List<GameObject> damagedSections = new List<GameObject>();
    public GameObject damageBlock;

  // Use this for initialization
  void Start () {
    health = 100;
    energy = 200;
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
        radius = gameObject.GetComponent<CapsuleCollider>().radius;
  }

  // Update is called once per frame
  void Update () {
    
    loseEnergy();
    
    // Update health and energy each frame
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
  }

  // lose the amount of health in the parameter
  void loseHealth(float damage){
    health -= damage;
    
    // Call the system break method
    SystemBreak();

    if(health < 0) {
      Destroy(gameObject);
    }
  }
  
  // lose 1 health
  void loseHealth() {
    health -= 1;
  }
  
  // lose the amount of energy in the parameter
  void loseEnergy(float energyLost) {
    energy -= energyLost;
  }
  
  // lose 1 energy
  void loseEnergy() {
    energy -= Time.deltaTime;
  }

  // Break a random system on the sub
  public void SystemBreak()
  {
        /* Your code here */
        // create a position for the damage to be spawned at
        Vector2 randomDirection = Random.insideUnitCircle.normalized * radius;
        float xLocation = Random.Range(-1.0f, 1.0f);
        Vector3 damagePosition = new Vector3(xLocation, randomDirection.x, randomDirection.y);

        // create the object
        GameObject temp = Instantiate(damageBlock, damagePosition, Quaternion.identity);
    }

  // On collision
  private void OnCollisionEnter(Collision collision)
  {
    if(collision.gameObject.tag == "fish")
    {
            loseHealth(5.0f);
        }
  }
}
