using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSwap : MonoBehaviour {
    
    public int whatcam;
    public int i = 0;
    public RenderTexture rendtex1, rendtex2;
	// Use this for initialization
	void Start () {
        //whatcam = 1;
        if (whatcam == 1)
        {
            gameObject.GetComponent<RawImage>().texture = rendtex1;
        }
        else
        {
            gameObject.GetComponent<RawImage>().texture = rendtex2;
        }
	}
	
	// Update is called once per frame
	void Update () {
        i += 1;
        if (Input.GetMouseButtonDown(0))
        {
            print("aaa");
            i = 0;
            if (whatcam == 1)
            {
                gameObject.GetComponent<RawImage>().texture = rendtex2;
                whatcam = 2;
            }
            else if (whatcam == 2)
            {
                gameObject.GetComponent<RawImage>().texture = rendtex1;
                whatcam = 1;
            }
        }
	}
}
