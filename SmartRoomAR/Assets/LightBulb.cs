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
    private GameObject panel;
    //[SerializeField]
    private float animationTime = 0.5f;

    //public Animator PanelSlider;
    

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
            //on = false;
            
        }
        else
        {
            EndTransition();
            panelCanvasGroup.alpha = 0;
            
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
        
        float targetX = 231;
        LeanTween.moveX(panelRectTransform, 432, 1.5f).setEase(LeanTweenType.easeOutBack);
        LeanTween.value(gameObject, UpdatePanelWidth, 0, 1000, animationTime).setEase(LeanTweenType.easeOutBack);
           //float panelWidth = panelRectTransform.rect.width;
           //float targetX = 231;
           //LeanTween.moveX(panelRectTransform, targetX, 1.5f).setEase(LeanTweenType.easeOutBack);

        //LeanTween.value(gameObject, UpdatePanelWidth, 0, 300, 1.5f).setEase(LeanTweenType.easeOutBack);
       

        
    }

    private void UpdatePanelWidth(float width)
    {
        panelRectTransform.sizeDelta = new Vector2(width, panelRectTransform.sizeDelta.y);
    }

    private void EndTransition()
    {
         //LeanTween.scaleX(panel, 0, 1.5f).setEase(LeanTweenType.easeOutBack);
        // LeanTween.moveX(panelRectTransform, 0, 1.5f).setEase(LeanTweenType.easeOutBack);
        LeanTween.value(gameObject, UpdatePanelWidth, 300, 0, 1.5f).setEase(LeanTweenType.easeOutBack);
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
