using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DischargePrompt : MonoBehaviour
{
    public Canvas mainScreen; // center of the screens inside the sub
    public GameObject panel; // panel that holds the emergency prompt
    public GameObject systemBreakPanel1;
    public GameObject systemBreakPanel2;
    public GameObject systemBreakPanel3;
    public Button SB1;
    public Button SB2;
    public Button SB3;
    private bool SB1Active = false;
    private bool SB2Active = false;
    private bool SB3Active = false;
    public Sprite warning;
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
    private SubVariables subVar; // reference to the submarine variables script
    // color variable
    private Color defaultColor;
    public float radius = 80.0f; // for discharge ability
    public SeekerFish[] seekerScripts; // list of all the fish in the scene
    public Slider energySlider;
    private AudioSource dischargeSound;

    // Use this for initialization
    void Start ()
    {
        // get a reference to the scripts
        subMovement = submarine.GetComponent<SubmarineMovement>();
        subVar = submarine.GetComponent<SubVariables>();

        // set boolean
        isBoosting = false;

        // turbo button color
        defaultColor = turbo.GetComponent<Image>().color;

        //fish = new List<GameObject>();
        seekerScripts = FindObjectsOfType<SeekerFish>();

        // add event listeners
        //discharge.GetComponent<Button>().onClick.AddListener(EmergencyPromptEnable);
        //yes.GetComponent<Button>().onClick.AddListener(EmergencyPromptDisable);
        //no.GetComponent<Button>().onClick.AddListener(EmergencyPromptDisable);

        //mainScreen.GetComponent<Button>().onClick.AddListener(EmergencyPromptEnable);
        dischargeSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        // change the image of the button on a system break
		if(subVar.systemBreak)
        {
            SB1.GetComponent<Image>().sprite = warning;
        }
	}

    // method to enable the emergency discharge prompt
    public void EmergencyPromptEnable()
    {
        Debug.Log("Clicked Emergency Button On");
        //panel.gameObject.GetComponent<Image>().enabled = true;

        // enable all of the children's Button components
        Button[] buttons = { };
        buttons = panel.gameObject.GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            button.enabled = true;
        }

        // enable all of the children's Text components
        Text[] texts = { };
        texts = panel.gameObject.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = true;
        }

        // enable all of the children's Image components
        Image[] images = { };
        images = panel.gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.enabled = true;
        }
    }

    // method to disable the emergency discharge prompt
    public void EmergencyPromptDisable()
    {
        Debug.Log("Clicked Emergency Button Off");

        // disable all of the children's Button components
        Button[] buttons = { };
        buttons = panel.gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        // disable all of the children's Text components
        Text[] texts = { };
        texts = panel.gameObject.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = false;
        }

        // disable all of the children's Image components
        Image[] images = { };
        images = panel.gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.enabled = false;
        }

        /*
        panel.gameObject.GetComponent<Image>().enabled = false;
        panel.gameObject.GetComponentInChildren<Button>().enabled = false;
        panel.gameObject.GetComponentInChildren<Text>().enabled = true;
        panel.gameObject.GetComponentInChildren<Image>().enabled = true;
        */
    }

    // happens if 'yes' is pressed after the emergency prompt pops up
    public void DischargeActivate()
    {
        // check to make sure there is enough energy to use
        if(energySlider.GetComponent<Slider>().value >= 40.0f)
        {
            Debug.Log("DISCHARGING!");

            // find all seekerfish scripts in the scene
            seekerScripts = FindObjectsOfType<SeekerFish>();

            // loop through all of the fish
            foreach (SeekerFish seekingFish in seekerScripts)
            {
                // stun the fish and then make them flee
                seekingFish.Stun(5.0f);
                seekingFish.Flee(submarine);
            }
            // deplete energy
            //energySlider.GetComponent<Slider>().value -= 40.0f; //40%
            subVar.loseEnergy(20.0f);

            // play sound
            dischargeSound.Play();
        }
        else
        {
            Debug.Log("Insufficient Energy To Discharge!");
        }
    }

    // method to toggle turbo
    public void ToggleTurbo()
    {
        Debug.Log("Turbo Toggled");
        // toggle the boolean
        if (isBoosting)
        {
            isBoosting = false;
            turbo.GetComponent<Image>().color = defaultColor;
        }
        else
        {
            isBoosting = true;
            turbo.GetComponent<Image>().color = Color.grey;
        }

        // update the submarineMovement script's corresponding boolean value
        subMovement.boosting = isBoosting;
    }

    // Methods for the side cameras
    // method to turn on/off high fps and light
    public void EnhanceScreenToggle()
    {
        GameObject testo = EventSystem.current.currentSelectedGameObject;
        testo.GetComponentInParent<CameraFPS>().HighFPS();
        testo.GetComponentInParent<LightsOn>().TurnOn();
        EventSystem.current = null;
        EventSystem.current.SetSelectedGameObject(null);
    }

    // method to enable system break 1
    public void SystemBreak1Clicked()
    {
        if (!SB1Active)
        {
            Debug.Log("Clicked SystemBreak1 Button On");
            //panel.gameObject.GetComponent<Image>().enabled = true;

            // enable all of the children's Button components
            Button[] buttons = { };
            buttons = systemBreakPanel1.gameObject.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.enabled = true;
            }

            // enable all of the children's Text components
            Text[] texts = { };
            texts = systemBreakPanel1.gameObject.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                text.enabled = true;
            }

            // enable all of the children's Image components
            Image[] images = { };
            images = systemBreakPanel1.gameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.enabled = true;
            }
            SB1Active = true;
            SB2Active = false;
            SB3Active = false;
        }
        else if (SB1Active)
        {
            SystemBreak1Disabled();
            SB1Active = false;
        }
        /*
        if (subVar.systemBreak)
        {

        }*/
    }

    // method to enable system break 2
    public void SystemBreak2Clicked()
    {
        if(!SB2Active)
        {
            Debug.Log("Clicked SystemBreak2 Button On");
            //panel.gameObject.GetComponent<Image>().enabled = true;

            // enable all of the children's Button components
            Button[] buttons = { };
            buttons = systemBreakPanel2.gameObject.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.enabled = true;
            }

            // enable all of the children's Text components
            Text[] texts = { };
            texts = systemBreakPanel2.gameObject.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                text.enabled = true;
            }

            // enable all of the children's Image components
            Image[] images = { };
            images = systemBreakPanel2.gameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.enabled = true;
            }
            SB1Active = false;
            SB2Active = true;
            SB3Active = false;
        }
        else if(SB2Active)
        {
            SystemBreak2Disabled();
            SB2Active = false;
        }
    }

    // method to enable system break 3
    public void SystemBreak3Clicked()
    {
        if(!SB3Active)
        {
            Debug.Log("Clicked SystemBreak3 Button On");
            //panel.gameObject.GetComponent<Image>().enabled = true;

            // enable all of the children's Button components
            Button[] buttons = { };
            buttons = systemBreakPanel3.gameObject.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.enabled = true;
            }

            // enable all of the children's Text components
            Text[] texts = { };
            texts = systemBreakPanel3.gameObject.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                text.enabled = true;
            }

            // enable all of the children's Image components
            Image[] images = { };
            images = systemBreakPanel3.gameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.enabled = true;
            }

            SB1Active = false;
            SB2Active = false;
            SB3Active = true;
        }
        else if(SB3Active)
        {
            SystemBreak3Disabled();
            SB3Active = false;
        }
    }

    // method to disable system break 1
    public void SystemBreak1Disabled()
    {
        Debug.Log("Clicked SystemBreak1 Button Off");
        //panel.gameObject.GetComponent<Image>().enabled = true;

        // enable all of the children's Button components
        Button[] buttons = { };
        buttons = systemBreakPanel1.gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        // enable all of the children's Text components
        Text[] texts = { };
        texts = systemBreakPanel1.gameObject.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = false;
        }

        // enable all of the children's Image components
        Image[] images = { };
        images = systemBreakPanel1.gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.enabled = false;
        }
    }

    // method to enable system break 2
    public void SystemBreak2Disabled()
    {
        Debug.Log("Clicked SystemBreak2 Button Off");
        //panel.gameObject.GetComponent<Image>().enabled = true;

        // enable all of the children's Button components
        Button[] buttons = { };
        buttons = systemBreakPanel2.gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        // enable all of the children's Text components
        Text[] texts = { };
        texts = systemBreakPanel2.gameObject.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = false;
        }

        // enable all of the children's Image components
        Image[] images = { };
        images = systemBreakPanel2.gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.enabled = false;
        }
    }

    // method to disable system break 3
    public void SystemBreak3Disabled()
    {
        Debug.Log("Clicked SystemBreak3 Button Off");
        //panel.gameObject.GetComponent<Image>().enabled = true;

        // enable all of the children's Button components
        Button[] buttons = { };
        buttons = systemBreakPanel3.gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        // enable all of the children's Text components
        Text[] texts = { };
        texts = systemBreakPanel3.gameObject.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = false;
        }

        // enable all of the children's Image components
        Image[] images = { };
        images = systemBreakPanel3.gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.enabled = false;
        }
    }
}
