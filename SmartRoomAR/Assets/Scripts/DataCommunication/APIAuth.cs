using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;


public class APIAuth : MonoBehaviour
{

    public static APIAuth Instance { get; private set; }
    private string base64auth;
    private string tempApiKey;
    public bool IsAuthenticated { get; private set;}

    public string AuthHeader { get; private set; }
    public string ApiKeyHeader { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    

    public void Authenticate(string username, string password, string apiKey)
    {
        base64auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        //Log base64auth to unity console
        Debug.Log(base64auth);



        tempApiKey = apiKey;
        StartCoroutine(SendTestRequest());
    }

    private IEnumerator SendTestRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://home.myopenhab.org/rest/things/zwave%3Adevice%3Ae804a908f8%3Anode14"))
        {
            www.SetRequestHeader("Authorization", $"Basic {base64auth}");
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Cookie", "CloudServer=10.11.0.33%3A3000; X-OPENHAB-AUTH-HEADER=true");
            www.SetRequestHeader("X-OPENHAB-TOKEN", tempApiKey);
            yield return www.SendWebRequest();

            //Log response to unity console
            Debug.Log(www.downloadHandler.text);

           if (www.result == UnityWebRequest.Result.Success) 
           {
                AuthHeader = $"Basic {base64auth}";
                ApiKeyHeader = tempApiKey;
                IsAuthenticated = true;

           }

              else
              {
                IsAuthenticated = false;
              }
        }
        
        OnAuthenticationComplete?.Invoke(IsAuthenticated);
        
    }

    
    public event Action<bool> OnAuthenticationComplete;

    

}
