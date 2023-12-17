using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private bool activated = false;

    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI container;
    


    public void ActivatePanel(string Title, string Container, bool Activate)
    {   
        activated = Activate;
        if(activated)
        {
            title.text = Title;
            container.text = Container;
            StartTransition();
            

        }
        else
        {
            activated = false;
            return;
        }
        activated = false;
    }


    private void StartTransition(){

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -2000);

        // Move the panel to the center of the screen
        LeanTween.moveY(rectTransform, -830, 1.5f).setEase(LeanTweenType.easeOutBack);
    }

    private void EndTransition(){
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 7);

        
        LeanTween.moveY(rectTransform, -2000, 1.5f).setEase(LeanTweenType.easeOutBack);
    }

    // Start is called before the first frame update
    void Start()
    {
        
       ActivatePanel("Title", "Container", true);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}