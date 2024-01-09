using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BuildARFromQRCode : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject objectToPlace; // AR object
    [SerializeField] private GameObject lampButtonPrefab; // Lamp Button UI Prefab
    [SerializeField] private Canvas uiCanvas; // Reference to UI Canvas
    [SerializeField] private QRCodeDetector qrCodeDetector; // Reference to your QR code detection script

    public event Action onListChanged;
    public List<GameObject> instantiatedObjects = new List<GameObject>();
    private const int maxRaycastAttempts = 50;
    private const float retryDelay = 0.5f; // Delay in seconds between raycast attempts

    void OnEnable()
    {
        lampButtonPrefab.SetActive(false);
        if (qrCodeDetector != null)
        {
            
            Debug.Log("QR Code Detector active.");
            qrCodeDetector.OnQRCodeDetected += HandleQRCodeDetected;
        }
        else
        {
            Debug.Log("qrCodeDetector is null");
        }
    }

    void OnDisable()
    {
        if (qrCodeDetector != null)
        {
            qrCodeDetector.OnQRCodeDetected -= HandleQRCodeDetected;
        }
    }

    private void HandleQRCodeDetected(string qrCodeData, Vector2? screenPosition)
    {
        if (qrCodeData == "deconz:extendedcolorlight:e7f460383d:ceiling-light-panel")
        {
            Debug.Log("Activating Lamp Button UI");
            lampButtonPrefab.SetActive(true);
           
        }

        if (screenPosition.HasValue)
        {
            Debug.Log("We have entered :)");
            StartCoroutine(RetryRaycast(qrCodeData, screenPosition.Value));
            
        }
    }

    private IEnumerator RetryRaycast(string qrCodeData, Vector2 screenPosition)
    {
        int attempts = 0;
        while (attempts < maxRaycastAttempts)
        {
            attempts++;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (raycastManager.Raycast(screenPosition, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                GameObject newObject = Instantiate(objectToPlace, hitPose.position, Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.current.transform.forward, Vector3.up)));

                newObject.name = qrCodeData;
                Debug.Log(newObject.name);
                instantiatedObjects.Add(newObject);
                Debug.Log("AR object instantiated at: " + hitPose.position);
                onListChanged?.Invoke();
                yield break; // Exit the coroutine if successful
            }
            else
            {
                Debug.Log("Raycast attempt " + attempts + " did not hit a plane for QR code: " + qrCodeData);
                yield return new WaitForSeconds(retryDelay); // Wait before retrying
            }
        }

        Debug.LogError("Raycast failed after " + maxRaycastAttempts + " attempts.");
    }

    


    public void UpdateARObjectWithData(string title, string content)
    {
        // Assuming we update the most recently added object
        if (instantiatedObjects.Count > 0)
        {
            GameObject objectToUpdate = instantiatedObjects[instantiatedObjects.Count - 1];

            InfoPanel arObjectController = objectToUpdate.GetComponent<InfoPanel>();

            if (arObjectController == null)
            {
                arObjectController = objectToUpdate.GetComponentInChildren<InfoPanel>();
            }

            if (arObjectController != null)
            {
                Debug.Log($"Updating InfoPanel with Title: {title}, Content: {content}");
                arObjectController.ActivatePanel(title, content, true);
            }
            else
            {
                Debug.LogError("InfoPanel component not found on the instantiated object or its children.");
            }
        }
        else
        {
            Debug.LogError("No instantiated object to update.");
        }

        Debug.Log("Instantiated Objects List:");
        foreach (GameObject obj in instantiatedObjects)
        {
            if (obj != null)
            {
                Debug.Log("Object: " + obj.name + ", Position: " + obj.transform.position);
            }
            else
            {
                Debug.Log("Null object in list");
            }
        }
    }

    public void RemoveSpecificARObject(GameObject objectToRemove)
    {
        if (instantiatedObjects.Contains(objectToRemove))
        {
            if(objectToRemove.name == "deconz:extendedcolorlight:e7f460383d:ceiling-light-panel")
            {
                lampButtonPrefab.SetActive(false);

            }
            instantiatedObjects.Remove(objectToRemove);
            Destroy(objectToRemove);
        }
        
    }

    public List<GameObject> GetInstantiatedObjects()
    {
        return instantiatedObjects;
    }

    public void RemoveAllARObjects()
    {
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
    }


}
