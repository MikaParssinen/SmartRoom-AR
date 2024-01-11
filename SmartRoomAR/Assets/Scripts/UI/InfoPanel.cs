
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


    [SerializeField]
    private RectTransform panelRectTransform;

    [SerializeField]
    private ContentSizeFitter contentSizeFitter;    


    public void ActivatePanel(string Title, string Container, bool Activate)
    {   
            title.text = Title;
            container.text = Container;
            AdjustSize();
    }

    private void AdjustSize()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(container.rectTransform);

        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRectTransform);

    }


    // Start is called before the first frame update
    void Start()
    {     
       ActivatePanel("Title", "Container", true);
       
    }

   

   
}

