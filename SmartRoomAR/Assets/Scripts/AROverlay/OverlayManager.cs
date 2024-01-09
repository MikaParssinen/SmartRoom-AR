using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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
        string title = data.label;
        string status = data.statusInfo.status;

        Debug.Log(title);
        Debug.Log(status);


        StartCoroutine(GetLinkedItemsStatus(data.channels, (channelsInfo) => {
            // Once all API responses are received, update the AR object
            if (buildARFromQRCode != null)
            {
                buildARFromQRCode.UpdateARObjectWithData(title, $"{status}\n{channelsInfo}");
            }
        }));
    }


    IEnumerator GetLinkedItemsStatus(List<APIManager.DeviceData.Channel> channels, Action<string> onCompleted)
    {
        string channelsInfo = "Channels: ";
        int totalCount = channels.SelectMany(ch => ch.linkedItems).Count();
        int processedCount = 0;

        foreach (var channel in channels)
        {
            foreach (var linkedItem in channel.linkedItems)
            {

                StartCoroutine(MakeApiCallForLinkedItem(linkedItem, (itemStatus) => {
                    channelsInfo += $"{linkedItem}: {itemStatus}, ";
                    processedCount++;

                    if (processedCount == totalCount)
                    {

                        channelsInfo = channelsInfo.TrimEnd(',', ' ');
                        onCompleted?.Invoke(channelsInfo);
                    }
                }));
            }
        }

        yield return null;
    }

    IEnumerator MakeApiCallForLinkedItem(string linkedItem, Action<string> onStatusReceived)
    {
        string apiUrl = "https://smart-room-worker.erik-ef2.workers.dev/getitemstate";
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        www.SetRequestHeader("item-name", linkedItem);

        yield return www.SendWebRequest(); // Wait for the response

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = www.downloadHandler.text;
            ItemStateResponse response = JsonUtility.FromJson<ItemStateResponse>(jsonResponse);
            onStatusReceived?.Invoke(response.state);
        }
        else
        {
            Debug.LogError("Error in API call: " + www.error);
            onStatusReceived?.Invoke("Error");
        }
    }

    [Serializable]
    public class ItemStateResponse
    {
        public string state;
    }
}