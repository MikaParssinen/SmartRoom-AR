using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DeviceControlManager : MonoBehaviour
{
    public static DeviceControlManager Instance;

    private void Awake()
    {
        //Create a singleton-instace
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //This is the function to send a command to the API
    public void SendCommandToApi(string apiEndpoint, string command)
    {
        StartCoroutine(SendCommandRoutine(apiEndpoint, command));
    }

    IEnumerator SendCommandRoutine(string apiEndpoint, string command)
    {
        // Here we want to use a POST-request to send a command to the API
        // We will be doing that by sending the AUTH-token along with the actual command
        string authToken = PlayerPrefs.GetString("AuthToken", "");

        WWWForm form = new WWWForm();
        form.AddField("Authorization", authToken);
        form.AddField("Command", command);

        using (UnityWebRequest www = UnityWebRequest.Post(apiEndpoint, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Command Send Error: " + www.error);
            }
            else
            {
                Debug.Log("Command sent successfully.");
            }
        }
    }
}
