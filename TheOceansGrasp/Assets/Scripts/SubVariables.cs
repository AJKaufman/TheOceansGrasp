using System.Collections;
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
        radius = gameObject.GetComponent<CapsuleCollider>().radius * 10.0f; // because the sub is scale 10
  }

  // Update is called once per frame
  void Update () {
    
    loseEnergy();
    
    // Update health and energy each frame
    displayedHealth.GetComponent<Slider>().value = health;
    displayedEnergy.GetComponent<Slider>().value = energy;
  }

  // lose the amount of health in the parameter
  public void loseHealth(float damage){
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
    }

  // On collision
  private void OnCollisionEnter(Collision collision)
  {
    
  }
}
