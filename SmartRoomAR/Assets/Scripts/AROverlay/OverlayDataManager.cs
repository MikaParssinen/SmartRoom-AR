using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiDataManager : MonoBehaviour
{
    public static event Action<string> OnApiDataReceived; //A event to notify that data has been recieved from the API

    public void FetchApiData(string apiEndpoint)
    {
        StartCoroutine(FetchDataRoutine(apiEndpoint));
    }

    IEnumerator FetchDataRoutine(string apiEndpoint)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiEndpoint))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("API Data Fetch Error: " + www.error);
            }
            else
            {
                //Handle the recieved data from the API
                string apiData = www.downloadHandler.text;
                Debug.Log("API Data received: " + apiData);

                //Send the data to the OverlayManager / OverlayManager can call this function to get all the data needed
                OnApiDataReceived?.Invoke(apiData);
            }
        }
    }
}