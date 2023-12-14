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
        redButton.onClick.AddListener(ChangeToRed);
        yellowButton.onClick.AddListener(ChangeToYellow);
        blueButton.onClick.AddListener(ChangeToBlue);  
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
            //Här ska animationen för att dra in Panelen köras
            //PanelSlider.SetTrigger("PlayAnimation");
        }
    }

    void ChangeToRed()
    {
        Debug.Log("Changed to red");
        lightSwitch.image.sprite = redLightBulb;
    }

    void ChangeToYellow()
    {
        Debug.Log("Changed to yellow");
        lightSwitch.image.sprite = yellowLightBulb;
    }

    void ChangeToBlue()
    {
        Debug.Log("Changed to blue");
        lightSwitch.image.sprite = blueLightBulb;
    }

/* Lägga till fler färger i fortsättningen
    void changeToGreen()
    {
        Debug.Log("Changed to green");
        lightSwitch.image.sprite = greenLightBulb;
    }

    void changeToWhite()
    {
        Debug.Log("Changed to white")
        lightSwitch.image.sprite = whiteLightBulb;
    }
    // Update is called once per frame
*/
    void Update()
    {

        
    }
}
