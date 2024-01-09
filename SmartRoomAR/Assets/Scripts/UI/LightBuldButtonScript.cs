using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightBulbButtonScript : MonoBehaviour
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
    private Button blueButton;

    [SerializeField]

    private Button greenButton;
    [SerializeField]
    private Button offButton;

    [SerializeField]
    //Panel in the canvas
    private GameObject panel;

    public APILampToggle apiLampToggle;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);

        lightSwitch.onClick.AddListener(ToggleColors);
        redButton.onClick.AddListener(ChangeToRed);
        blueButton.onClick.AddListener(ChangeToBlue);
        greenButton.onClick.AddListener(changeToGreen);
        offButton.onClick.AddListener(changeToOff);

    }

    void ToggleColors()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }

    void ChangeToRed()
    {
        apiLampToggle.SendCommand("red");
        lightSwitch.GetComponent<Image>().sprite = redLightBulb;
        panel.SetActive(false);
    }

    void ChangeToBlue()
    {
        apiLampToggle.SendCommand("blue");
        lightSwitch.GetComponent<Image>().sprite = blueLightBulb;
        panel.SetActive(false);
    }

    void changeToGreen()
    {
        apiLampToggle.SendCommand("green");
        lightSwitch.GetComponent<Image>().sprite = greenLightBulb;
        panel.SetActive(false);
    }

    void changeToOff()
    {
        apiLampToggle.SendCommand("off");
        lightSwitch.GetComponent<Image>().sprite = whiteLightBulb;
        panel.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {

    }
}