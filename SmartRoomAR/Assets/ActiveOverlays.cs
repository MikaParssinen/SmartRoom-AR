using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOverlays : MonoBehaviour
{
    [SerializeField]

    private string[] activeOverlays;

    [SerializeField]
    private Button overlayButton;

    [SerializeField]
    private GameObject overlayPanel;
    public RectTransform overlayPanelRectTransform;



    
    //Detta kommer att behöva stå i den klassen som anropar denna!
    //För att göra överföringen av overlays så säker som möjligt
    /* 
    public class OtherClass
    {
        public static event Action<string[]> OnActiveOverlaysChanged;

        private void SomeMethod()
        {
            string[] activeOverlays = GetActiveOverlays();
            OnActiveOverlaysChanged?.Invoke(activeOverlays);
        }
    }
    */
    private void InitiateOverlays(string[] data)
    {
        if(data.Length == 0 || data == null)
        {
            return;
        }
        else
        {
            activeOverlays = data;
        }

    }
    private void OnClick()
    {
        
    }

    private void StartTransition()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {   
        overlayPanelRectTransform = overlayPanel.GetComponent<RectTransform>();
        overlayButton.onClick.AddListener(OnClick);
        //Denna rad gör invoken möjlig!
        //OtherClass.OnActiveOverlaysChanged += InitiateOverlays;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
