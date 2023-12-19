using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;


public class UIDHandler : MonoBehaviour
{
    // Reference to the QRCodeDetector script
    public QRCodeDetector qrCodeDetector;

    // Reference to the ApiManager script
    public ApiManager apiManager;

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
        uid = "zwave:device:e804a908f8:node14";
        string transformedUid = TransformUid(uid);

        // Send the transformed UID to the API manager for further processing
        apiManager.SendRequest(transformedUid);
    }

    private string TransformUid(string uid)
    {
        // Replace ":" with "%3"
        return uid.Replace(":", "%3A");
    }

    public class ApiManager : MonoBehaviour
    {
        private const string apiUrl = "https://home.myopenhab.org/rest/things/";

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

                    // Now you can send this data to other scripts or handle it accordingly
                    HandleApiData(responseData);
                }
            }
        }

        private void HandleApiData(string responseData)
        {
            // Implement logic to handle the API response data here
            //parse the data and use it as needed
        }
    }
}
