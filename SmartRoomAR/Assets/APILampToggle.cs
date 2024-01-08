using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class APILampToggle : MonoBehaviour
{
    private readonly string workerApiUrl = "https://smart-room-worker.erik-ef2.workers.dev/sendcommand";

    public void SendCommand(string command)
    {

        StartCoroutine(SendWorkerCommand(command));

    }

    private IEnumerator SendWorkerCommand(string command)
    {

        string commandType = DetermineCommandType(command);
        string commandValue = PreparePayload(command);

        if (commandType == null || commandValue == null)
        {
            Debug.Log("Command not found");
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequest.Get(workerApiUrl))
        {
            www.SetRequestHeader("command-type", commandType);
            www.SetRequestHeader("command-value", commandValue);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Command sent");
            }


        }

    }


    private string DetermineCommandType(string command)
    {

        switch (command)
        {
            case "on":
            case "off":
                return command.ToLower();
            case "red":
            case "blue":
            case "green":
            case "yellow":
                return "colour";
            default:
                return null;
        }


    }

    private string PreparePayload(string command)
    {
        var commandPresets = new Dictionary<string, string>
        {
           {"on", "ON"},
           {"off", "OFF"},
           //Colours in HSB format
           {"red", "0, 100, 100"},
           {"blue", "240, 100, 100"},
           {"green", "120, 100, 100"},
           {"yellow", "60, 100, 100"},
        };

        if (commandPresets.TryGetValue(command, out string payload))
        {
            return payload;
        }
        else
        {
            return null;
        }

    }
}


