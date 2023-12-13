using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightBulb : MonoBehaviour
{
    public Sprite whiteLightBulb;
    public Sprite yellowLightBulb;
    public Sprite redLightBulb;
    public Sprite greenLightBulb;
    public Sprite blueLightBulb;


    [SerializeField]
    private Button lightSwitch;
    [SerializeField]
    private Button redButton;
    [SerializeField]
    private Button yellowButton;
    [SerializeField]
    private Button blueButton;

    public Animator PanelSlider;
    

    [SerializeField]
    private bool on = false;

   
    // Start is called before the first frame update
    void Start()
    {
        lightSwitch.onClick.AddListener(ToggleColors); 
        //redButton.onClick.AddListener(ChangeToRed);
        //yellowButton.onClick.AddListener(ChangeToYellow);
        //blueButton.onClick.AddListener(ChangeToBlue);  
    }

    void ToggleColors()
    {
        on = !on;
        if (on)
        {
            Debug.Log("Button pressed!!!!");
            PanelSlider.SetTrigger("PlayAnimation");
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
