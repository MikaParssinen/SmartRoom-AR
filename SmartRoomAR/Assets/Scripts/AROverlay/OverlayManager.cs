using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public ReadAndStore readAndStore; // Reference to ReadAndStore script
    public InfoPanel infoPanelPrefab; // Reference to InfoPanel prefab

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

    private void HandleQRCodeDetected(DeviceDataPacket dataPacket)
    {
        string additionalInfo = readAndStore.GetAdditionalInfo(dataPacket.Label);

        ActivateInfoPanel("Title", additionalInfo);
    }

    private void ActivateInfoPanel(string title, string container)
    {
        // Instantiate a new InfoPanel with the provided information
        InfoPanel infoPanelInstance = Instantiate(infoPanelPrefab);
        
        // Activate the InfoPanel with the provided information
        infoPanelInstance.ActivatePanel(title, container, true);
    }
}
