using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveOverlay : MonoBehaviour
{

    public GameObject overlayPrefab;
    public Transform contentPanel;
    public GameObject panel;
    public BuildARFromQRCode buildARFromQRCode;


    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        UpdateOverlayList();
        
    }

    // Update is called once per frame
    private void OnEnable()
    {
       buildARFromQRCode.onListChanged += UpdateOverlayList;
    }

    private void OnDisable()
    {
        buildARFromQRCode.onListChanged -= UpdateOverlayList;
    }

    public void UpdateOverlayList()
    {
        var currentObjects = buildARFromQRCode.GetInstantiatedObjects();
       
        // Clear the list



        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject arObject in currentObjects)
        {
            GameObject newOverlay = Instantiate(overlayPrefab, contentPanel);
            newOverlay.GetComponentInChildren<TextMeshProUGUI>().text = arObject.name;

            var toggle = newOverlay.GetComponentInChildren<Toggle>();
            toggle.isOn = true;
            toggle.onValueChanged.AddListener(isChecked =>
            {
                // If unchecked, remove the overlay
                if (!isChecked)
                {
                    buildARFromQRCode.RemoveSpecificARObject(arObject);
                    UpdateOverlayList(); // Refresh the UI
                }
            });
        }
       
    }
}
