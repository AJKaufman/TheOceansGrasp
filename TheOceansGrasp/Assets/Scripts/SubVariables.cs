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
    public GameObject damageBlock;
    public GameObject goalObject;
    public Camera frontCamera;
    private Vector3 offSetSubmarinePos;
    private SubmarineMovement submarineMovement;
    public GameObject techNodeParent; // gameobject that holds other tech nodes
    public List<GameObject> techNodes; // list of tech nodes
    //public GameObject damageNode;
    public Material damage1;
    public Material damage2;
    public Material damage3;
    private Material newDamageAppearance;
    // system break variables
    private DamageBlockRemoval damageRemoval;
    public bool systemBreak = false;
    public int systemBreakNum = 0;
    private float smallDamage = 5.0f;
    private float largeDamage = 20.0f;
    public float damageBeforeSystemBreak = 0.0f;
    public int totalDamageNodes = 0;
    public int totalRepairsMade = 0;
    public Button systemBreak1Button;
    public GameObject systemBreak1Panel;
    public GameObject player;
    public GameObject win;
    public GameObject lose;

  // Use this for initialization
    void Start () {
    health = 100;
    energy = 200;
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
        progress.GetComponent<Slider>().value = percent;
        radius = gameObject.GetComponent<CapsuleCollider>().radius * 10.0f; // because the sub is scale 10

        // populate the list of tech nodes
        techNodes = new List<GameObject>();
        foreach(Transform child in techNodeParent.GetComponent<Transform>())
        {
            // add a damage script to it
            //child.gameObject.AddComponent<DamageBlockRemoval>();
            // add it to the list
            techNodes.Add(child.gameObject);
        }

        // calculate starting distance
        startDistance = Vector3.Distance(frontCamera.transform.position, goalObject.transform.position);

        // reference to the sub movement script
        submarineMovement = gameObject.GetComponent<SubmarineMovement>();
    }

  // Update is called once per frame
  void Update () {
    
        // check for repairs
        /*
        if(totalRepairsMade >= 4)
        {
            damageBeforeSystemBreak = 0.0f;
            totalRepairsMade = 0;
        }*/

   // calculate the distance
   distance = Vector3.Distance(frontCamera.transform.position, goalObject.transform.position);
   //distance = Vector3.Distance(gameObject.transform.position+offSetSubmarinePos, goalObject.transform.position);
    // convert to a percentage
    percent = ((startDistance - distance) / startDistance)*100.0f;
        Debug.Log("Health = " + health);
    if(distance <= 10)
        {
            win.SetActive(true);
            player.SetActive(false);
        }
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

    if(health <= 0) {
            health = 0;
            lose.SetActive(true);
            player.SetActive(false);

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
        if (energy > 0.0f)
        {
            energy -= Time.deltaTime;
        }
        else
        {
            energy = 0.0f;
        }
  }

    public void gainEnergy()
    {
        if(energy <= 200.0f)
        {
            energy += Time.deltaTime * 3.0f;
        }
        else
        {
            energy = 200.0f;
        }
    }

  // Break a random system on the sub
  public void SystemBreak()
  {
        /* Your code here */

        // create a random number to indicate which tech node breaks
        int nodeIndex = Random.Range(0, techNodes.Count);
        // get a reference to the specific node's damage removal script
        damageRemoval = techNodes[nodeIndex].GetComponent<DamageBlockRemoval>();
        damageRemoval.isDamaged = true;
        // save the transform data before swapping the mesh
        Transform tempTransform = damageRemoval.gameObject.GetComponent<Transform>();
        // change the model to be visually different
        int randomModel = Random.Range(0, 3);
        if(randomModel == 0)
        {
            newDamageAppearance = damage1;
        }
        if (randomModel == 1)
        {
            newDamageAppearance = damage2;
        }
        if (randomModel == 2)
        {
            newDamageAppearance = damage3;
        }
        //damageRemoval.gameObject.GetComponent<MeshFilter>().sharedMesh = damage1.GetComponent<Transform>().GetChild(0).GetComponent<Mesh>();
        // re-instantiate the transform data
        //damageRemoval.gameObject.GetComponent<Transform>().position = tempTransform.position;
        //damageRemoval.gameObject.GetComponent<Transform>().rotation = tempTransform.rotation;

        damageRemoval.GetComponent<MeshRenderer>().material = newDamageAppearance;
        //damageRemoval.GetComponent<MeshRenderer>().material.color = Color.red;
        damageRemoval.GetComponent<DamageBlockRemoval>().isDamaged = true;
        Positions.instance.damagedNodes.Add(damageRemoval.gameObject);
        if (Positions.instance.outside)
        {
            damageRemoval.transform.parent = null;
        }

        /*
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
        */

        // keep track of the number of damage nodes
        if (damageBeforeSystemBreak >= smallDamage)
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
            // sub speed change
            submarineMovement.halfSpeed = true;
            submarineMovement.quadSpeed = false;
            submarineMovement.eightSpeed = false;

            // change the button and panel's text
            systemBreak1Button.GetComponentInChildren<Text>().text = "Stage 1";
            systemBreak1Panel.GetComponentInChildren<Text>().text = "Engine Damaged. You are now at half of your regular speed.";
        }
        else if(submarineMovement.halfSpeed)
        {
            // sub speed change
            submarineMovement.halfSpeed = false;
            submarineMovement.quadSpeed = true;
            submarineMovement.eightSpeed = false;

            systemBreak1Button.GetComponentInChildren<Text>().text = "Stage 2";
            systemBreak1Panel.GetComponentInChildren<Text>().text = "Engine Further Damaged. You are now at 1/4th of your regular speed.";
        }
        else if(submarineMovement.quadSpeed)
        {
            // sub speed change
            submarineMovement.halfSpeed = false;
            submarineMovement.quadSpeed = false;
            submarineMovement.eightSpeed = true;

            systemBreak1Button.GetComponentInChildren<Text>().text = "Stage 3";
            systemBreak1Panel.GetComponentInChildren<Text>().text = "Engine Severely Damaged. You are now at 1/8th of your regular speed.";
        }
        else
        {
            // make the sub move at half speed
            submarineMovement.halfSpeed = false;
            submarineMovement.quadSpeed = false;
            submarineMovement.eightSpeed = false;

            // change the button and panel's text
            systemBreak1Button.GetComponentInChildren<Text>().text = "Fixed";
            systemBreak1Panel.GetComponentInChildren<Text>().text = "Engine Repaired. You are now at your regular speed.";
        }
    }

  // On collision
  private void OnCollisionEnter(Collision collision)
  {
    
  }
}
