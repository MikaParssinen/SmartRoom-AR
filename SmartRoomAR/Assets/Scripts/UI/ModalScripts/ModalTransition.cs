using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModalTransition : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -2000);

        // Move the panel to the center of the screen
        LeanTween.moveY(rectTransform, 7, 0.5f).setEase(LeanTweenType.easeOutBack);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
