using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class APILampToggle : MonoBehaviour
{
    private readonly string apiUrlTurnOn = "https://home.myopenhab.org/rest/items/Ceiling_Lamp_switch_21_01";
    private readonly string apiUrlTurnOff = "https://home.myopenhab.org/rest/items/Ceiling_Lamp_switch_21_01";
    private readonly string apiUrlState = "https://home.myopenhab.org/rest/items/Ceiling_Lamp_switch_21_01/state";
    private readonly string apiUrlChangeColour = "https://home.myopenhab.org/rest/items/Ceiling_Lamp_color_21_03";


    public void SendCommand(string command)
    {
       
        StartCoroutine(CheckStateAndSendCommand(command));

         

        
    }

    private IEnumerator CheckStateAndSendCommand(string command)
    {
        //Get current state of the lamp
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrlState))
        {
            //Set headers
            www.SetRequestHeader("Authorization", APIAuth.Instance.AuthHeader);
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Cookie", "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true");
            www.SetRequestHeader("X-OPENHAB-TOKEN", APIAuth.Instance.ApiKeyHeader);

            //Send request
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                //Log error to unity console
                Debug.Log("Error getting state");
            }
            else
            {
                //If the lamp is on but command is off
                if(www.downloadHandler.text == "ON" && command == "off")
                {
                    yield return StartCoroutine(SendTurnOffCommand());
                }

                Debug.Log(www.downloadHandler.text);
                if(www.downloadHandler.text == "OFF")
                {
                    yield return StartCoroutine(SendTurnOnCommand());
                }
                //Now set the colour command 
                yield return StartCoroutine(SendSetColourCommand(PreparePayload(command)));
                   

                
            }

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

    private IEnumerator SendTurnOnCommand()
    {
        //Post request to openhab API that takes the payload as string in the body
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrlTurnOn, ""))
        {
            //Set headers
            www.SetRequestHeader("Authorization", APIAuth.Instance.AuthHeader);
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Cookie", "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true");
            www.SetRequestHeader("X-OPENHAB-TOKEN", APIAuth.Instance.ApiKeyHeader);

            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("ON"));
            www.uploadHandler.contentType = "text/plain";

            www.downloadHandler = new DownloadHandlerBuffer();

            //Send request
            yield return www.SendWebRequest();

            //Log response to unity console
            Debug.Log(www.downloadHandler.text);

            //Check if request was successful
            if (www.result == UnityWebRequest.Result.Success)
            {
                //Log success to unity console
                Debug.Log("Command sent successfully");
            }
            else
            {
                //Log error to unity console
                Debug.Log("Error sending command");
            }

        }

    }

    private IEnumerator SendTurnOffCommand()
    {
        //Post request to openhab API that takes the payload as string in the body
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrlTurnOff, ""))
        {
            //Set headers
            www.SetRequestHeader("Authorization", APIAuth.Instance.AuthHeader);
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Cookie", "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true");
            www.SetRequestHeader("X-OPENHAB-TOKEN", APIAuth.Instance.ApiKeyHeader);

            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("OFF"));
            www.uploadHandler.contentType = "text/plain";

            www.downloadHandler = new DownloadHandlerBuffer();
            //Send request
            yield return www.SendWebRequest();

            //Log response to unity console
            Debug.Log(www.downloadHandler.text);

            //Check if request was successful
            if (www.result == UnityWebRequest.Result.Success)
            {
                //Log success to unity console
                Debug.Log("Command sent successfully");
            }
            else
            {
                //Log error to unity console
                Debug.Log("Error sending command");
            }

        }
    
    }

    private IEnumerator SendSetColourCommand(string payload)
    {
        //Post request to openhab API that takes the payload as string in the body
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrlChangeColour, ""))
        {
            //Set headers
            www.SetRequestHeader("Authorization", APIAuth.Instance.AuthHeader);
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Cookie", "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true");
            www.SetRequestHeader("X-OPENHAB-TOKEN", APIAuth.Instance.ApiKeyHeader);

            //Set payload
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
            www.uploadHandler.contentType = "text/plain";

            www.downloadHandler = new DownloadHandlerBuffer();

            //Send request
            yield return www.SendWebRequest();

            //Log response to unity console
            Debug.Log(www.downloadHandler.text);

            //Check if request was successful
            if (www.result == UnityWebRequest.Result.Success)
            {
                //Log success to unity console
                Debug.Log("Command sent successfully");
            }
            else
            {
                //Log error to unity console
                Debug.Log("Error sending command");
            }

        }

    }

}


