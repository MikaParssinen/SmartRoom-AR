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

    [SerializeField]
    private GameObject overlayDataPrefab;

    
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
        if(data == null || data.Length == 0)
        {
            Debug.Log("No overlays to initiate");
            return;
        }
        else
        {
            
            activeOverlays = data;

            Transform content = overlayPanelRectTransform.Find("Scroll View/Viewport/Content");
            Debug.Log("Active overlays length: " + activeOverlays.Length);
            foreach(string overlay in activeOverlays)
            {
                GameObject instance = Instantiate(overlayDataPrefab);
                instance.transform.SetParent(content, false);
                instance.transform.localScale = new Vector3(0.1f, 0.01f, 0.01f); // Set the scale of the prefab
                instance.transform.localPosition = Vector3.zero; // Set the local position of the prefab
                instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = overlay;

            }
            StartTransition();
        }

    }
    private void OnClick()
    {
        
    }

    private void StartTransition()
    {
        RectTransform rectTransform = overlayPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -2000);

        // Move the panel to the center of the screen
        LeanTween.moveY(rectTransform, -200, 1.5f).setEase(LeanTweenType.easeOutBack);
    }
    
    // Start is called before the first frame update
    void Start()
    {   
        overlayPanelRectTransform = overlayPanel.GetComponent<RectTransform>();
        overlayButton.onClick.AddListener(OnClick);
        InitiateOverlays(new string[] { "Overlay1", "Overlay2", "Overlay3" });
        //Denna rad gör invoken möjlig!
        //OtherClass.OnActiveOverlaysChanged += InitiateOverlays;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
