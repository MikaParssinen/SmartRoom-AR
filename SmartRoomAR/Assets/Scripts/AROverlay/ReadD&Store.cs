using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class UIDHandler : MonoBehaviour
{
    public static event Action<string, DeviceProperties> OnDeviceInfoReceived;
    // Reference to the QRCodeDetector script
    public QRCodeDetector qrCodeDetector;

    // Reference to the ApiManager script
    public ApiManager apiManager;

    // Class to represent dynamic device properties
    [Serializable]
    public class DeviceProperties
    {
        public string status;
        public string label;
    }

    // Dictionary to store dynamic device properties using UID as the key
    private Dictionary<string, DeviceProperties> devicePropertiesDictionary = new Dictionary<string, DeviceProperties>();

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
        apiManager.SendRequest(transformedUid);
    }

    private string TransformUid(string uid)
    {
        // Replace ":" with "%3A"
        return uid.Replace(":", "%3A");
    }

    // Method to store dynamic device properties in the dictionary
    public void StoreDeviceProperties(string uid, DeviceProperties properties)
    {
        devicePropertiesDictionary[uid] = properties;
    }

    public class ApiManager : MonoBehaviour
    {
        private const string apiUrl = "https://home.myopenhab.org/rest/things/";
        public APIAuth apiAuth;

        public void SendRequest(string uid)
        {
            StartCoroutine(GetDeviceInfo(uid));
        }

        IEnumerator GetDeviceInfo(string uid)
{
    string requestUrl = $"{apiUrl}?uid={uid}";
    Debug.Log("The full url is: " + requestUrl);

    using (UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl))
    {
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

            // Deserialize the JSON response to a generic dictionary
            var responseDict = JsonUtility.FromJson<Dictionary<string, object>>(responseData);

            // Extract only the relevant information
            DeviceProperties deviceProperties = new DeviceProperties
            {
                status = GetStringValue(responseDict, "status"),
                label = GetStringValue(responseDict, "label")
            };

            // Store the dynamic device properties in the dictionary
            StoreDeviceProperties(uid, deviceProperties);

            // Send this data to other scripts or handle it accordingly
            HandleApiData(uid, deviceProperties);
        }
    }
}

        // Helper method to get string value from dictionary
        private string GetStringValue(Dictionary<string, object> dict, string key)
        {
            return dict.ContainsKey(key) ? dict[key].ToString() : null;
        }

        // Helper method to get color value from dictionary
        private Color GetColorValue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                // Parse color string and convert it to Color
                ColorUtility.TryParseHtmlString(dict[key].ToString(), out Color color);
                return color;
            }
            return Color.white; // Default color if not found
        }

        // Helper method to get float value from dictionary
        private float GetFloatValue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key) && float.TryParse(dict[key].ToString(), out float value))
            {
                return value;
            }
            return 0f; // Default value if not found or cannot be parsed
        }

        private void HandleApiData(DeviceProperties deviceProperties)
        {
            // Store the dynamic device properties in the dictionary
            StoreDeviceProperties(uid, deviceProperties);

            // Send this data to other scripts or handle it accordingly
            UIDHandler.OnDeviceInfoReceived?.Invoke(uid, deviceProperties);
        }
    }
}
