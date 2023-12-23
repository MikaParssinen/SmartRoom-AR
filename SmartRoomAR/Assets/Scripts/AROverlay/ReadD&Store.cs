using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UIDHandler : MonoBehaviour
{
    // Reference to the QRCodeDetector script
    public QRCodeDetector qrCodeDetector;

    // Reference to the ApiManager script
    public ApiManager apiManager;

    // Reference to the APIAuth script
    public APIAuth apiAuth;

    private void OnEnable()
    {
        // Subscribe to the QR code detection event
        qrCodeDetector.OnQRCodeDetected += HandleQRCodeDetected;
    }

    private void OnDisable()
    {
        // Unsubscribe from the QR code detection event
        qrCodeDetector.OnQRCodeDetected -= HandleQRCodeDetected;
    }

    private void HandleQRCodeDetected(string uid)
    {
        string transformedUid = TransformUid(uid);

        // Send the transformed UID to the API manager for further processing
        apiManager.SendRequest(transformedUid, apiAuth.GetHeaders());
    }

    private string TransformUid(string uid)
    {
        // Replace ":" with "%3"
        return uid.Replace(":", "%3A");
    }
}

public class ApiManager : MonoBehaviour
{
    private const string apiUrl = "https://home.myopenhab.org/rest/things/";

    // Reference to the APIAuth script
    public APIAuth apiAuth;

    public void SendRequest(string uid, Dictionary<string, string> headers)
    {
        StartCoroutine(GetDeviceInfo(uid, headers));
    }

    IEnumerator GetDeviceInfo(string uid, Dictionary<string, string> headers)
    {
        string requestUrl = $"{apiUrl}?uid={uid}";
        Debug.Log("The full url is: " + requestUrl);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl))
        {
            // Set headers from APIAuth
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                // Parse and use the response data as needed
                string responseData = webRequest.downloadHandler.text;
                Debug.Log($"API Response: {responseData}");

                // Send the response data to the HandleApiData method in UIDHandler
                UIDHandler uidHandler = FindObjectOfType<UIDHandler>();
                if (uidHandler != null)
                {
                    uidHandler.HandleApiData(responseData);
                }
            }
        }
    }

    // Method to handle the DeviceDataPacket
    public void HandleDeviceData(UIDHandler.DeviceDataPacket dataPacket)
    {
        // Handling the data packet here
        Debug.Log($"Device Label: {dataPacket.Label}");
        Debug.Log($"Device Status: {dataPacket.Status}");
        foreach (var channel in dataPacket.Channels)
        {
            Debug.Log($"Channel: {channel.Key}, Description: {channel.Value}");
        }
    }
}

