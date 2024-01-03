using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Linq;

[Serializable]
public class AuthResponse
{
    public string token;
}
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
    

    public void Authenticate(string username, string password)
    {
        base64auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        //Log base64auth to unity console
        Debug.Log(base64auth);
        StartCoroutine(SendTestRequest(username));
    }

    public bool Validate(string token) {

        //Log token to unity console
        Debug.Log(token);

        //Set headers
       
        ApiKeyHeader = $"Bearer {token}";

        //Log headers to unity console
      
        Debug.Log(ApiKeyHeader);

        //Send request
        StartCoroutine(SendValidateRequest());

        return IsAuthenticated;
    }

    private IEnumerator SendValidateRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://smart-room-worker.erik-ef2.workers.dev/validate"))
        {
            www.SetRequestHeader("Authorization", ApiKeyHeader);
            

            yield return www.SendWebRequest();

            //Log response to unity console
            Debug.Log(www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                IsAuthenticated = true;
            }

            else
            {
                IsAuthenticated = false;
            }
        }

        OnAuthenticationComplete?.Invoke(IsAuthenticated);

    }



    private IEnumerator SendTestRequest(string username)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://smart-room-worker.erik-ef2.workers.dev/login"))
        {
            www.SetRequestHeader("Authorization", $"Basic {base64auth}");
            //Send the username in the body of the request
            www.SetRequestHeader("username", username);

            yield return www.SendWebRequest();

            //Log response to unity console
            Debug.Log(www.downloadHandler.text);

           if (www.result == UnityWebRequest.Result.Success) 
           {
                IsAuthenticated = true;
                AuthResponse jsonResponse = JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);
                
                string token = jsonResponse.token;

                PlayerPrefs.SetString("token", token);

                Debug.Log(PlayerPrefs.GetString("notoken"));


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
