using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class SmartRoomData
{
    public string deviceId;
    public string deviceType;
    //Will have to change these later based on what we actually want to recieve from the openHAB API
}

public class SmartRoomController : MonoBehaviour
{
    
    private string apiUrl = "https://home.myopenhab.org/settings/things/";

    void Start()
    {
        StartCoroutine(GetSmartRoomData());
    }

    IEnumerator GetSmartRoomData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // Handle the JSON response
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received data: " + jsonResponse);

                // Transform the JSON response to a C# object (IF NEEDED)
                SmartRoomData smartRoomData = JsonUtility.FromJson<SmartRoomData>(jsonResponse);

                // Update what we want to be done with the device
                UpdateUI(smartRoomData);

                // Send a command to the specific device (IF NEEDED)
                SendCommandToSmartDevice(smartRoomData.deviceId, "on");
            }
        }
    }

    // This should be the function to send the command to the specific device inside the smartroom
    void SendCommandToSmartDevice(string deviceId, string command)
    {
        // Implement this to actually send a command to the device
    }
}
