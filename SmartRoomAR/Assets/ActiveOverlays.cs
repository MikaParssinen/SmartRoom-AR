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
    private IEnumerator InitiateOverlays(string[] data)
    {
        if(data == null || data.Length == 0)
        {
            Debug.Log("No overlays to initiate");
            yield break;
        }
        else
        {
            activeOverlays = data;

            Transform content = overlayPanelRectTransform.Find("Scroll View/Viewport/Content");

            foreach(string overlay in activeOverlays)
            {
                GameObject instance = Instantiate(overlayDataPrefab);
                instance.transform.SetParent(content, false);
                instance.transform.localScale = new Vector3(0.1f, 0.01f, 0.01f); // Set the scale of the prefab
                instance.transform.localPosition = Vector3.zero; // Set the local position of the prefab
                instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = overlay;
            }
            //RectTransform contentRectTransform = content.GetComponent<RectTransform>();
            // Wait for a frame to let the layout group do its work
            yield return new WaitForEndOfFrame();

            VerticalLayoutGroup layoutGroup = content.GetComponent<VerticalLayoutGroup>();

            // Calculate the extra scroll space
            float extraScrollPerPrefab = 4f; // Adjust this value to your needs
            float extraScrollSpace = activeOverlays.Length * extraScrollPerPrefab;

            // Add the extra scroll space to the bottom padding of the VerticalLayoutGroup
            layoutGroup.padding.bottom = (int)extraScrollSpace;

            StartTransition();
        }
    }
    private void OnClick()
    {
        //InitiateOverlays(new string[] { "Overlay1", "Overlay2", "Overlay3" });
        string[] testArray = new string[] {"Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10"};
        
        //string[] testArray = new string[] {"Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10", "Value11", "Value12", "Value13", "Value14", "Value15", "Value16", "Value17", "Value18", "Value19", "Value20", "Value21", "Value22", "Value23", "Value24", "Value25"};
        StartCoroutine(InitiateOverlays(testArray));
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
        
        

        VerticalLayoutGroup layoutGroup = overlayPanelRectTransform.Find("Scroll View/Viewport/Content").GetComponent<VerticalLayoutGroup>();
        layoutGroup.padding.top = 50; // Change the top padding
        layoutGroup.spacing = 30; // Change the spacing between elements
        layoutGroup.childAlignment = TextAnchor.UpperCenter; 
            
            
        //Denna rad gör invoken möjlig!
        //OtherClass.OnActiveOverlaysChanged += InitiateOverlays;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
