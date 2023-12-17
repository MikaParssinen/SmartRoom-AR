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
    [SerializeField]

    private Button greenButton;

    [SerializeField]

    private Button whiteButton;

    [SerializeField]
    private GameObject panel;
    
    private float animationTime = 0.5f;

    
    

    [SerializeField]
    private bool on = false;

    public CanvasGroup panelCanvasGroup;

    public RectTransform panelRectTransform;

   
    // Start is called before the first frame update
    void Start()
    {
        panelRectTransform = panel.GetComponent<RectTransform>();
        panelRectTransform.sizeDelta = new Vector2(0, panelRectTransform.sizeDelta.y);

        lightSwitch.onClick.AddListener(ToggleColors); 
        redButton.onClick.AddListener(ChangeToRed);
        yellowButton.onClick.AddListener(ChangeToYellow);
        blueButton.onClick.AddListener(ChangeToBlue);  
        greenButton.onClick.AddListener(changeToGreen);
        whiteButton.onClick.AddListener(changeToWhite);
    }

    void Awake()
    {
         
        panelCanvasGroup.alpha = 0;
    }

    void ToggleColors()
    {
        on = !on;
        if (on)
        {
            Debug.Log("Button pressed!!!!");
            panelCanvasGroup.alpha = 1;
            
            StartTransition();
            blueButton.interactable = true;
            redButton.interactable = true;
            yellowButton.interactable = true;
            
            
        }
        else
        {
            EndTransition();
            //panelCanvasGroup.alpha = 0;
            
        }
    }



    IEnumerator ChangeWidth(RectTransform panel, float startWidth, float endWidth, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            float newWidth = Mathf.Lerp(startWidth, endWidth, (elapsedTime / time));
            panel.sizeDelta = new Vector2(newWidth, panel.sizeDelta.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.sizeDelta = new Vector2(endWidth, panel.sizeDelta.y);
    }


    private void StartTransition()
    {
        
        float initialX = 455;
        panelRectTransform.localPosition = new Vector2(initialX, panelRectTransform.localPosition.y);
        LeanTween.moveX(panelRectTransform, 455, 1.5f).setEase(LeanTweenType.easeOutBack);
        LeanTween.value(gameObject, UpdatePanelWidth, 0, 2054, animationTime).setEase(LeanTweenType.easeOutBack);
          
       

        
    }

    private void UpdatePanelWidth(float width)
    {
        panelRectTransform.sizeDelta = new Vector2(width, panelRectTransform.sizeDelta.y);
    }

    private void EndTransition()
    {

         
        float finalX = 455; 
        LeanTween.moveX(panelRectTransform, finalX, 1.5f).setEase(LeanTweenType.easeInBack);
        StartCoroutine(ChangeWidth(panelRectTransform, panelRectTransform.rect.width, 0, animationTime));
        LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 0, 0.01f).setEase(LeanTweenType.easeInBack);
        yellowButton.interactable = false;
        redButton.interactable = false;
        blueButton.interactable = false;
        
    }

    

    void ChangeToRed()
    {
        Debug.Log("Changed to red");
        lightSwitch.image.sprite = redLightBulb;
        EndTransition();
    }

    void ChangeToYellow()
    {
        Debug.Log("Changed to yellow");
        lightSwitch.image.sprite = yellowLightBulb;
       EndTransition();
    }

    void ChangeToBlue()
    {
        Debug.Log("Changed to blue");
        lightSwitch.image.sprite = blueLightBulb;
        EndTransition();
    }


    void changeToGreen()
    {
        Debug.Log("Changed to green");
        lightSwitch.image.sprite = greenLightBulb;
        EndTransition();
    }

    void changeToWhite()
    {
        Debug.Log("Changed to white");
        lightSwitch.image.sprite = whiteLightBulb;
        EndTransition();
    }



    // Update is called once per frame
    void Update()
    {

        
    }
}
