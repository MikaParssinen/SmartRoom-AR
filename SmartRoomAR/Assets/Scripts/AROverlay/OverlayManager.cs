using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public ReadAndStore readAndStore; // Reference to ReadAndStore script
    public InfoPanel infoPanel; // Reference to InfoPanel script

    private void OnEnable()
    {
        // Subscribe to the event in ReadAndStore that signals a new QR code detection
        readAndStore.GetDeviceInfo += HandleQRCodeDetected;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when this script is disabled
        readAndStore.GetDeviceInfo -= HandleQRCodeDetected;
    }

    private void HandleQRCodeDetected(string uid)
    {
        string additionalInfo = readAndStore.GetAdditionalInfo(uid);

        ActivateInfoPanel("Title", additionalInfo);
    }

    private void ActivateInfoPanel(string title, string container)
    {
        // Assuming you have a method in InfoPanel to activate it with the provided information
        infoPanel.ActivatePanel(title, container, true);
    }
}
