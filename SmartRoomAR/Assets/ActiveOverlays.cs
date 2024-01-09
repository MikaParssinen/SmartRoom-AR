using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOverlays : MonoBehaviour
{
    [SerializeField] private Button overlayButton;
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private GameObject overlayDataPrefab;

    public RectTransform overlayPanelRectTransform;

    private void Start()
    {
        overlayPanelRectTransform = overlayPanel.GetComponent<RectTransform>();
        overlayButton.onClick.AddListener(OnClick);

        VerticalLayoutGroup layoutGroup = overlayPanelRectTransform.Find("Scroll View/Viewport/Content").GetComponent<VerticalLayoutGroup>();
        layoutGroup.padding.top = 50; // Change the top padding
        layoutGroup.spacing = 30; // Change the spacing between elements
        layoutGroup.childAlignment = TextAnchor.UpperCenter;

        //BuildARFromQRCode.OnActiveOverlaysChanged += InitiateOverlays;
    }

    private void OnDisable()
    {
        //BuildARFromQRCode.OnActiveOverlaysChanged -= InitiateOverlays;
    }

    private void OnClick()
    {
        List<string> testList = new List<string> { "Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10" };
        StartCoroutine(InitiateOverlays(testList));
    }

    private IEnumerator InitiateOverlays(List<string> data)
    {
        if (data == null || data.Count == 0)
        {
            Debug.Log("No overlays to initiate");
            yield break;
        }

        Transform content = overlayPanelRectTransform.Find("Scroll View/Viewport/Content");

        foreach (string overlay in data)
        {
            GameObject instance = Instantiate(overlayDataPrefab);
            instance.transform.SetParent(content, false);
            instance.transform.localScale = new Vector3(0.1f, 0.01f, 0.01f);
            instance.transform.localPosition = Vector3.zero;
            instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = overlay;
        }

        yield return new WaitForEndOfFrame();

        VerticalLayoutGroup layoutGroup = content.GetComponent<VerticalLayoutGroup>();
        float extraScrollPerPrefab = 4f; // Adjust as needed
        float extraScrollSpace = data.Count * extraScrollPerPrefab;
        layoutGroup.padding.bottom = (int)extraScrollSpace;

        StartTransition();
    }

    private void StartTransition()
    {
        RectTransform rectTransform = overlayPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -2000);
        LeanTween.moveY(rectTransform, -200, 1.5f).setEase(LeanTweenType.easeOutBack);
    }
}
