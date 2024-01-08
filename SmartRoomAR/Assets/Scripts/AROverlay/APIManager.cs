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

                // Convert the JSON response to DeviceData
                DeviceData data = JsonUtility.FromJson<DeviceData>(responseData);

                // Send the data to the listeners (OverlayManager)
                OnApiDataReceived?.Invoke(data);
            }
        }
    }

    // Method to handle the DeviceData
    public void HandleDeviceData(DeviceData data)
    {
        // Handling the data here
        Debug.Log($"Device Label: {data.Label}");
        Debug.Log($"Device Status: {data.StatusInfo.Status}");
        foreach (var channel in data.Channels)
        {
            Debug.Log($"Channel: {channel.ChannelName}, Description: {channel.Description}");
        }
    }

    [Serializable]
    public struct DeviceData
    {
        public string Label;
        public StatusInfo StatusInfo;
        public List<Channel> Channels;
    }

    [Serializable]
    public struct StatusInfo
    {
        public string Status;
    }

    [Serializable]
    public struct Channel
    {
        // Define the properties you get for each channel in JSON format here
        // Example:
        public string ChannelName;
        public string Description;
    }
}