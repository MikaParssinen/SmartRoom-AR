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
    private RectTransform panel;
    //[SerializeField]
    private float animationTime = 0.5f;

    //public Animator PanelSlider;
    

    [SerializeField]
    private bool on = false;

    public CanvasGroup panelCanvasGroup;

   
    // Start is called before the first frame update
    void Start()
    {
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
            
        }
        else
        {
            EndTransition();
            panelCanvasGroup.alpha = 0;
            
        }
    }

    IEnumerator SlidePanel(RectTransform panel, Vector2 start, Vector2 end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            panel.position = Vector2.Lerp(start, end, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.position = end;
    }

    private void StartTransition()
    {
        Vector2 startPos = lightSwitch.transform.position;
        Vector2 endPos = startPos - new Vector2(231, 0);
        StartCoroutine(SlidePanel(panel, startPos, endPos, animationTime));
    }

    private void EndTransition()
    {
        Vector2 endPos = lightSwitch.transform.position;
        Vector2 startPos = endPos + new Vector2(panel.rect.width, 0);
        StartCoroutine(SlidePanel(panel, startPos, endPos, animationTime));
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
