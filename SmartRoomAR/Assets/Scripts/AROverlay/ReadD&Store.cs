using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]
public class SmartRoomData
{
    public string label;
    public string bridgeUID;
    public Configuration configuration;
    public Properties properties;
    public string UID;
    public string thingTypeUID;
    public string location;
    public List<Channel> channels;
    public StatusInfo statusInfo;
    public FirmwareStatus firmwareStatus;
    public bool editable;

    // Constructor to initialize the properties
    public SmartRoomData(string label, string bridgeUID, Configuration configuration, Properties properties, string UID, string thingTypeUID, string location, List<Channel> channels, StatusInfo statusInfo, FirmwareStatus firmwareStatus, bool editable)
    {
        this.label = label;
        this.bridgeUID = bridgeUID;
        this.configuration = configuration;
        this.properties = properties;
        this.UID = UID;
        this.thingTypeUID = thingTypeUID;
        this.location = location;
        this.channels = channels;
        this.statusInfo = statusInfo;
        this.firmwareStatus = firmwareStatus;
        this.editable = editable;
    }

    // Nested classes for proper JSON serialization
    [System.Serializable]
    public class Configuration
    {
        public object additionalProp1;
        public object additionalProp2;
        public object additionalProp3;
    }

    [System.Serializable]
    public class Properties
    {
        public string additionalProp1;
        public string additionalProp2;
        public string additionalProp3;
    }

    [System.Serializable]
    public class Channel
    {
        public string uid;
        public string id;
        public string channelTypeUID;
        public string itemType;
        public string kind;
        public string label;
        public string description;
        public List<string> defaultTags;
        public Properties properties;
        public Configuration configuration;
        public string autoUpdatePolicy;
        public List<string> linkedItems;
    }

    [System.Serializable]
    public class StatusInfo
    {
        public string status;
        public string statusDetail;
        public string description;
    }

    [System.Serializable]
    public class FirmwareStatus
    {
        public string status;
        public string updatableVersion;
    }
}


public class SmartRoomController : MonoBehaviour
{
    private string apiUrl = "https://home.myopenhab.org/rest/things/";
    public event Action<string, SmartRoomData> OnSmartRoomDataReceived;

    void Start()
    {
        /*QRCodeDetector qrCodeDetector = FindObjectOfType<QRCodeDetector>();
        if (qrCodeDetector != null)
        {
            qrCodeDetector.OnQRCodeDetected += HandleQRCodeDetected;
        }
        else
        {
            Debug.LogError("QRCodeDetector not found in the scene.");
        }*/
    }

    private void SendCommandToSmartDevice(string uid, string command)
    {
        // Implement the logic to send a command to the smart device
        // You can use the uid and command parameters to send the appropriate command
        Debug.Log($"Sending command to device {uid}: {command}");
        // Add your implementation here
    }
    // Event handler for QR code detection
    private void HandleQRCodeDetected(string qrCodeData)
    {
        // Parse the QR code data and extract the UID
        string[] parts = qrCodeData.Split(':');
        string uid = parts[2];

        // Use the UID to call the API and get device information
        StartCoroutine(GetSmartRoomData(uid));
    }

   IEnumerator GetSmartRoomData(string uid)
{
    // Replace colons with URL-encoded equivalent
    string escapedUid = Uri.EscapeDataString(uid.Replace(":", "%3"));

    // Modify the URL to include the variable name for the UID
    string fullApiUrl = apiUrl + "?uid=" + escapedUid;
    Debug.Log(fullApiUrl);

    using (UnityWebRequest www = UnityWebRequest.Get(fullApiUrl))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            SmartRoomData smartRoomData = JsonUtility.FromJson<SmartRoomData>(jsonResponse);

            // Trigger the event when data is received, passing both uid and data
            OnSmartRoomDataReceived?.Invoke(uid, smartRoomData);

            // Update UI and send command as needed
            UpdateUI(smartRoomData);
            SendCommandToSmartDevice(uid, "on");
        }
    }
}

    void UpdateUI(SmartRoomData data)
    {
        Debug.Log("Device ID: " + data.UID);
        Debug.Log("Device Type: " + data.thingTypeUID);
        Debug.Log("Label: " + data.label);
    }
}
