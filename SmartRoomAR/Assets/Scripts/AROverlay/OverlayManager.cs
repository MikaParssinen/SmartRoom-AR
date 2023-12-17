using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    [SerializeField]
    private SmartRoomController smartRoomController; // Reference to ReadD&Store script

    [SerializeField]
    private InfoPanel infoPanel;

    private void Start()
    {
        smartRoomController = FindObjectOfType<SmartRoomController>();

        if (smartRoomController != null)
        {
            smartRoomController.OnSmartRoomDataReceived += HandleSmartRoomDataReceived;
        }
        else
        {
            Debug.LogError("SmartRoomController not found in the scene.");
        }
    }

    private void HandleSmartRoomDataReceived(string uid, SmartRoomData smartRoomData)
    {
        // Implement your logic here to handle the received data
        // You can access UID and SmartRoomData properties
        Debug.Log($"Received data for UID {uid}: {smartRoomData.label}");

        // Extract information from SmartRoomData and pass it to InfoPanel
        string title = "Device: " + smartRoomData.UID;
        string container = "Type: " + smartRoomData.thingTypeUID + "\nLabel: " + smartRoomData.label;

        // Activate the InfoPanel
        infoPanel.ActivatePanel(title, container, true);
    }
}
