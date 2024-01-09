using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class APIManager : MonoBehaviour
{
    public event Action<DeviceData> OnApiDataReceived;

    private const string apiUrl = "https://home.myopenhab.org/rest/things/";

    // Reference to the APIAuth script
    public APIAuth apiAuth;

    public void SendRequest(string uid)
    {
        StartCoroutine(GetDeviceInfo(uid));
    }

    IEnumerator GetDeviceInfo(string uid)
    {
        string requestUrl = $"{apiUrl}?uid={uid}";
        Debug.Log("The full URL is: " + requestUrl);

        using (UnityWebRequest www = UnityWebRequest.Get("https://smart-room-worker.erik-ef2.workers.dev/retrieveinfo"))
        {
            www.SetRequestHeader("UID", uid);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Parse and use the response data as needed
                string responseData = www.downloadHandler.text;
                Debug.Log($"API Response: {responseData}");

                // Convert the JSON response to DeviceData using JsonUtility
                DeviceData data = JsonUtility.FromJson<DeviceData>(responseData);

                // Send the data to the listeners (OverlayManager)
                OnApiDataReceived?.Invoke(data);
            }
        }
    }

    // Method to handle the DeviceData
    public void HandleDeviceData(DeviceData data)
    {
        // Access the fields you need
        foreach (var channel in data.channels)
        {
            Debug.Log($"Channel: {channel.label}, Description: {channel.description}");
        }

        Debug.Log($"Device Label: {data.label}");
        Debug.Log($"Device Status: {data.statusInfo.status}");
    }

    [Serializable]
    public class DeviceData
    {
        public List<Channel> channels;
        public StatusInfo statusInfo;
        public string label;

        [Serializable]
        public class Channel
        {
            public string uid;
            public string label;
            public string description;
        }

        [Serializable]
        public class StatusInfo
        {
            public string status;
            public string statusDetail;
        }
    }
}