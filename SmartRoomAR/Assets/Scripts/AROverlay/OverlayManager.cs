using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public ReadAndStore readAndStore; // Reference to ReadAndStore script
    public InfoPanel infoPanel; // Reference to InfoPanel script

    private void OnEnable()
    {
        // Subscribe to the event in UIDHandler that signals new device information
        UIDHandler.OnDeviceInfoReceived += HandleDeviceInfoReceived;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when this script is disabled
        UIDHandler.OnDeviceInfoReceived -= HandleDeviceInfoReceived;
    }

    private void HandleDeviceInfoReceived(string uid, DeviceProperties deviceProperties)
    {
        // Handle the received device information
        string additionalInfo = $"{deviceProperties.status}, {deviceProperties.color}, {deviceProperties.runtime}";
        
        // Update the UI or perform any other actions
        ActivateInfoPanel("Title", additionalInfo);
    }

    private void ActivateInfoPanel(string title, string container)
    {
        // Assuming you have a method in InfoPanel to activate it with the provided information
        infoPanel.ActivatePanel(title, container, true);
    }
}
