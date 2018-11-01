﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubVariables : MonoBehaviour {

  float health;
  float energy;
    float startDistance = 0.0f;
    float distance = 0.0f;
    float percent = 0.0f;
  public Slider displayedHealth;
  public Slider displayedEnergy;
    public Slider progress;
    float radius;
    public List<GameObject> damagedSections = new List<GameObject>();
    public GameObject damageBlock;
    public GameObject goalObject;
    public Camera frontCamera;
    private Vector3 offSetSubmarinePos;
    private SubmarineMovement submarineMovement;

    // system break variables
    public bool systemBreak = false;
    private float smallDamage = 5.0f;
    private float largeDamage = 20.0f;
    private float damageBeforeSystemBreak = 0.0f;
    private int totalDamageNodes = 0;
    private int totalRepairsMade = 0;
    public Button systemBreak1Button;
    public GameObject systemBreak1Panel;

  // Use this for initialization
    void Start () {
    health = 100;
    energy = 200;
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
        progress.GetComponent<Slider>().value = percent;
        radius = gameObject.GetComponent<CapsuleCollider>().radius * 10.0f; // because the sub is scale 10

        // calculate starting distance
        startDistance = Vector3.Distance(frontCamera.transform.position, goalObject.transform.position);

        // create an offset for the position in the submarine that the distance calculation will run against
        //offSetSubmarinePos = new Vector3(0.0f, 0.0f, 10.0f);

        submarineMovement = gameObject.GetComponent<SubmarineMovement>();
    }

  // Update is called once per frame
  void Update () {
    
    loseEnergy();

   // calculate the distance
   distance = Vector3.Distance(frontCamera.transform.position, goalObject.transform.position);
   //distance = Vector3.Distance(gameObject.transform.position+offSetSubmarinePos, goalObject.transform.position);
    // convert to a percentage
    percent = ((startDistance - distance) / startDistance)*100.0f;
        //Debug.Log("Percent = " + percent);

    // Update health and energy each frame
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
    progress.GetComponent<Slider>().value = percent;
    }

  // lose the amount of health in the parameter
  public void loseHealth(float damage){
    health -= damage;
        damageBeforeSystemBreak += damage;
    // Call the system break method
    SystemBreak();

    if(health < 0) {
      Destroy(gameObject);
    }
  }
  
  // lose 1 health
  public void loseHealth() {
    health -= 1;
  }
  
  // lose the amount of energy in the parameter
  public void loseEnergy(float energyLost) {
    energy -= energyLost;
        Debug.Log("Current Energy: " + energy);
  }
  
  // lose 1 energy
  public void loseEnergy() {
    energy -= Time.deltaTime;
  }

  // Break a random system on the sub
  public void SystemBreak()
  {
        /* Your code here */
        // create a position for the damage to be spawned at
        Vector2 randomDirection = Random.insideUnitCircle.normalized * radius; // at which point around the circular part of the hull
        float zLocation = Random.Range(-5.0f, 5.0f); // length of the sub

        // set the position of the damage in relation to the submarine
        Vector3 currentSubPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Vector3 damagePosition = new Vector3(currentSubPos.x + randomDirection.x, currentSubPos.y + randomDirection.y, currentSubPos.z + zLocation);

        // create the object
        GameObject temp = Instantiate(damageBlock, damagePosition, Quaternion.identity);

        // add the object to a list
        damagedSections.Add(temp);

        // set the parent of the object to be the submarine so that the damage moves with it
        temp.transform.parent = gameObject.transform;

        // keep track of the number of damage nodes
        if(damageBeforeSystemBreak >= smallDamage)
        {
            totalDamageNodes++;
        }
        if(damageBeforeSystemBreak >= largeDamage)
        {
            systemBreak = true;
        }

        // if more damage than the value of large damage has been dealt a system break will occur that causes the speed of the submarine to drop
        if(systemBreak)
        {
            // make the sub move at half speed
            submarineMovement.halfSpeed = true;

            // change the button and panel's text
            systemBreak1Button.GetComponentInChildren<Text>().text = "Active";
            systemBreak1Panel.GetComponentInChildren<Text>().text = "Engine Damaged. You are now at half of your regular speed.";
        }
    }

  // On collision
  private void OnCollisionEnter(Collision collision)
  {
    
  }
}
