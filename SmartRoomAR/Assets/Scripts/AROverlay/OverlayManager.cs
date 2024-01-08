using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public APIManager apiManager; // Reference to your API Manager
    public BuildARFromQRCode buildARFromQRCode; // Reference to the BuildARFromQRCode script

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
        Debug.Log($"This is the data in HandleApiDataReceived: {data} ");
     
        // Extract the information from the API data
        string title = data.Label;
        string status = data.StatusInfo.Status;

        Debug.Log(title);
        Debug.Log(status);

        

        // Construct additional information about channels
        string channelsInfo = "Channels: ";
        foreach (var channel in data.Channels)
        {
            channelsInfo += $"{channel.ChannelName}, ";
        }

        // Remove the trailing comma and space
        channelsInfo = channelsInfo.TrimEnd(',', ' ');

        // Update the AR object with the new data
        if (buildARFromQRCode != null)
        {
            Debug.Log("Updating infopanel");
            buildARFromQRCode.UpdateARObjectWithData(title, $"{status}\n{channelsInfo}");
        }
    }
}