using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubVariables : MonoBehaviour {

  float health;
  float energy;
  public Slider displayedHealth;
  public Slider displayedEnergy;

  // Use this for initialization
  void Start () {
    health = 100;
    energy = 200;
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
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


    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Colliding!");

        if(collision.gameObject.tag == "fish")
        {
            loseHealth(5.0f);
            Debug.Log("Current Health: " + health);
        }
    }


}
