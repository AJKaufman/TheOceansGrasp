using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubVariables : MonoBehaviour {

  float health = 100;
  float energy = 100;
  public GameObject displayedHealth;

  // Use this for initialization
  void Start () {
    displayedHealth.Slider.Set(health, false);
  }

  // Update is called once per frame
  void Update () {

  }

  // lose the amount of health in the parameter
  void loseHealth(float damage){
    health -= damage;
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
    energy -= 1;
  }
  
}
