using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AuthenticationManager : MonoBehaviour
{
    private string apiUrl = "blank"; // Replace with the exact URL to gather the API AUTH token
    private string authTokenKey = "AuthToken";

    // Call this function from somewhere when you need the authentication token
    public void AttemptLogin(string username, string password)
    {
        StartCoroutine(Login(username, password));
    }

    IEnumerator Login(string username, string password)
    {
        // Create a POST request with username and password
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Login Error: " + www.error);
            }
            else
            {
                // Receive and store the credentials for authentication
                string authToken = www.GetResponseHeader("Authorization");
                PlayerPrefs.SetString(authTokenKey, authToken);

                Debug.Log("Login successful. AuthToken: " + authToken);
            }
        }
    }

    // This function can be called from any other script that needs to get the API authentication
    public string GetAuthToken()
    {
        // Access AuthHeader from APIAuth.Instance
        if (APIAuth.Instance != null)
        {
            return APIAuth.Instance.AuthHeader;
        }
        else
        {
            Debug.LogError("APIAuth instance not found.");
            return "";
        }
    }
}
