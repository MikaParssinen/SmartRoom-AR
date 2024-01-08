using UnityEngine;
using System.Collections;
using System;


public class OverlayManager : MonoBehaviour
{
    public APIManager apiManager;

    [SerializeField]
    private GameObject infoPanelPrefab; // Has to be GameObject since InfoPanel is a prefab

    private void OnEnable()
    {
        // Subscribe to the event in APIManager that signals new API data
        apiManager.OnApiDataReceived += HandleApiDataReceived;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when this script is disabled
        apiManager.OnApiDataReceived -= HandleApiDataReceived;
    }

    private void HandleApiDataReceived(APIManager.DeviceData data)
    {
        // Directly use the information from the data
        string title = data.Label;
        string status = data.StatusInfo.Status;

        // Construct additional information about channels
        string channelsInfo = "Channels: ";
        foreach (var channel in data.Channels)
        {
            channelsInfo += $"{channel.ChannelName}, ";
        }

        // Remove the trailing comma and space
        channelsInfo = channelsInfo.TrimEnd(',', ' ');

        ActivateInfoPanel(title, $"{status}\n{channelsInfo}");
    }

    private void ActivateInfoPanel(string title, string container)
    {
        // Instantiate a new InfoPanel prefab with the provided information
        GameObject infoPanelInstance = Instantiate(infoPanelPrefab);

        // Access the InfoPanel script on the instantiated GameObject
        InfoPanel infoPanel = infoPanelInstance.GetComponent<InfoPanel>();

        if (infoPanel != null)
        {
            // Activate the InfoPanel with the provided information
            infoPanel.ActivatePanel(title, container, true);
        }
        else
        {
            Debug.LogError("infoPanel script not found on the instantiated GameObject.");
        }
    }
}