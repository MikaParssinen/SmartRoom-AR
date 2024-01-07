
using UnityEngine;
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
           
        }
        else
        {

            //activated = false;

            return;
        }
       // activated = false;
    }


    // Start is called before the first frame update
    void Start()
    {     
       ActivatePanel("Title", "Container", true);
       
    }

   

   
}

