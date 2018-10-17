using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DischargePrompt : MonoBehaviour
{
    public Canvas mainScreen; // center of the screens inside the sub
    private GameObject panel; // panel that holds the emergency prompt
    private GameObject damageRight; // right and left damage panels, corresponding to their screen directions
    private GameObject damageLeft;
    private GameObject brokenRight; // right and left broken camera panels, corresponding to their screen directions
    private GameObject brokenLeft;
    public Button discharge;
    public Button turbo;
    public Button yes;
    public Button no;
    private bool isBoosting; // boolean used to toggle turbo
    public GameObject submarine; // reference to the submarine object
    private SubmarineMovement subMovement; // reference to the submarine movement script

	// Use this for initialization
	void Start ()
    {
        // get a reference to the script
        subMovement = submarine.GetComponent<SubmarineMovement>();

        // set boolean
        isBoosting = false;

        // get the panel
        panel = GameObject.Find("EmergencyPrompt");
        damageLeft = GameObject.Find("DamageScreenLeft");
        damageRight = GameObject.Find("DamageScreenRight");
        brokenLeft = GameObject.Find("BrokenScreenLeft");
        brokenRight = GameObject.Find("BrokenScreenRight");

        // add event listeners
        //discharge.GetComponent<Button>().onClick.AddListener(EmergencyPromptEnable);
        //yes.GetComponent<Button>().onClick.AddListener(EmergencyPromptDisable);
        //no.GetComponent<Button>().onClick.AddListener(EmergencyPromptDisable);

        //mainScreen.GetComponent<Button>().onClick.AddListener(EmergencyPromptEnable);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    // method to enable the emergency discharge prompt
    public void EmergencyPromptEnable()
    {
        Debug.Log("Clicked Emergency Button On");
        panel.gameObject.GetComponent<Image>().enabled = true;
        panel.gameObject.GetComponentInChildren<Button>().enabled = true;
    }

    // method to disable the emergency discharge prompt
    public void EmergencyPromptDisable()
    {
        Debug.Log("Clicked Emergency Button Off");
        panel.gameObject.GetComponent<Image>().enabled = false;
        panel.gameObject.GetComponentInChildren<Button>().enabled = false;
    }

    // method to toggle turbo
    public void ToggleTurbo()
    {
        Debug.Log("Turbo Toggled");
        print("Turbo Toggled");
        // toggle the boolean
        if (isBoosting)
        {
            isBoosting = false;
        }
        else
        {
            isBoosting = true;
        }

        // update the submarineMovement script's corresponding boolean value
        subMovement.boosting = isBoosting;
    }

    // Methods for the side cameras
    // method to enable the screen damage panel on the right screen
    public void DamageScreenRightEnable()
    {
        damageRight.gameObject.GetComponent<Image>().enabled = true;
    }
    // method to disable the screen damage panel on the right screen
    public void DamageScreenRightDisable()
    {
        damageRight.gameObject.GetComponent<Image>().enabled = false;
    }
    // method to enable the camera broken panel on the right screen
    public void BrokenScreenRightEnable()
    {
        brokenRight.gameObject.GetComponent<Image>().enabled = true;
        brokenRight.GetComponentInChildren<Text>().enabled = true;
    }
    // method to disable the camera broken panel on the right screen
    public void BrokenScreenRightDisable()
    {
        brokenRight.gameObject.GetComponent<Image>().enabled = false;
        brokenRight.GetComponentInChildren<Text>().enabled = false;
    }
    // method to turn on/off high fps and the light for the right screen
    public void EnhanceScreenRightToggle()
    {
        
    }

    // method to enable the screen damage panel on the left screen
    public void DamageScreenLeftEnable()
    {
        damageLeft.gameObject.GetComponent<Image>().enabled = true;
    }
    // method to disable the screen damage panel on the left screen
    public void DamageScreenLeftDisable()
    {
        damageLeft.gameObject.GetComponent<Image>().enabled = false;
    }
    // method to enable the camera broken panel on the left screen
    public void BrokenScreenLeftEnable()
    {
        brokenLeft.gameObject.GetComponent<Image>().enabled = true;
        brokenLeft.GetComponentInChildren<Text>().enabled = true;
    }
    // method to disable the camera broken panel on the left screen
    public void BrokenScreenLeftDisable()
    {
        brokenLeft.gameObject.GetComponent<Image>().enabled = false;
        brokenLeft.GetComponentInChildren<Text>().enabled = false;
    }
    // method to turn on/off high fps and the light for the left screen
    public void EnhanceScreenLeftToggle()
    {

    }
}
