using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AuthenticationManager : MonoBehaviour
{
    private string apiUrl = "blank"; //Should be replaced with the exact URL to gather the API AUTH token
    private string authTokenKey = "AuthToken";

    void Start()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        // Make a POST-request without sending any credentials and only expecting a Authentication Token
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, ""))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Login Error: " + www.error);
            }
            else
            {
                //Recieve and gather the credentials for the authentication
                string authToken = www.GetResponseHeader("Authorization");
                PlayerPrefs.SetString(authTokenKey, authToken);

                Debug.Log("Login successful. AuthToken: " + authToken);
            }
        }
    }

    //This function can be called from any other script that need to get the API authentication
    public string GetAuthToken()
    {
        return PlayerPrefs.GetString(authTokenKey, "");
    }
}
