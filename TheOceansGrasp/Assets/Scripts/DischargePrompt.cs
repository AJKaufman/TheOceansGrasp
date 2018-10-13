using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DischargePrompt : MonoBehaviour
{
    public Canvas mainScreen; // center of the screens inside the sub
    private GameObject panel; // panel that holds the emergency prompt
    public Button discharge;
    public Button turbo;
    public Button yes;
    public Button no;

	// Use this for initialization
	void Start ()
    {
        // get the panel
        panel = GameObject.Find("EmergencyPrompt");

        // add event listeners
        discharge.GetComponent<Button>().onClick.AddListener(EmergencyPromptEnable);
        yes.GetComponent<Button>().onClick.AddListener(EmergencyPromptDisable);
        no.GetComponent<Button>().onClick.AddListener(EmergencyPromptDisable);

        //mainScreen.GetComponent<Button>().onClick.AddListener(EmergencyPromptEnable);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // method to enable the prompt
    void EmergencyPromptEnable()
    {
        panel.gameObject.GetComponent<Image>().enabled = true;
        panel.gameObject.GetComponentInChildren<Button>().enabled = true;
    }

    void EmergencyPromptDisable()
    {
        panel.gameObject.GetComponent<Image>().enabled = false;
        panel.gameObject.GetComponentInChildren<Button>().enabled = false;
    }
}
