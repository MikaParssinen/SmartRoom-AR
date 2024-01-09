using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BuildARFromQRCode : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject objectToPlace; // Your AR object
    [SerializeField] private GameObject lampButtonPrefab; // Your Lamp Button UI Prefab
    [SerializeField] private Canvas uiCanvas; // Reference to your UI Canvas
    [SerializeField] private QRCodeDetector qrCodeDetector; // Reference to your QR code detection script

    public event Action onListChanged;
    public List<GameObject> instantiatedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void OnEnable()
    {

        if(qrCodeDetector != null)
        {
            Debug.Log("Trying to PlaceObject");
            qrCodeDetector.OnQRCodeDetected += PlaceObjectInAR;
        }
        else
        {
            Debug.Log("qrCodeDetector is null");
        }
        
    }

    void OnDisable()
    {
        if(qrCodeDetector != null)
        {
            qrCodeDetector.OnQRCodeDetected -= PlaceObjectInAR;

        }
        
    }

    private void PlaceObjectInAR(string qrCodeData, Vector2? screenPosition)
    {
        Debug.Log("PlaceObjectInAR called with QR Code Data: " + qrCodeData);

        if (qrCodeData == "deconz:extendedcolorlight:e7f460383d:ceiling-light-panel")
        {
            // Instantiate the lamp button UI
            Debug.Log("Activating Lamp Button UI");
            GameObject lampButtonUI = Instantiate(lampButtonPrefab, uiCanvas.transform, false);
            lampButtonUI.name = "Lamp Button UI";
        }

        // AR object instantiation logic with retry mechanism
        const int maxAttempts = 50; // Maximum number of raycast attempts
        int attempts = 0;
        bool hitSuccess = false;

        while (attempts < maxAttempts && !hitSuccess)
        {
            attempts++;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (screenPosition.HasValue && raycastManager.Raycast(screenPosition.Value, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                GameObject newObject = Instantiate(objectToPlace, hitPose.position, Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.current.transform.forward, Vector3.up)));
                newObject.name = qrCodeData;
                instantiatedObjects.Add(newObject);
                Debug.Log("Instantiated new AR object at: " + hitPose.position);
                hitSuccess = true;
            }
            else
            {
                Debug.Log("Raycast attempt " + attempts + " did not hit a plane for QR code: " + qrCodeData);
            }
        }

        if (!hitSuccess)
        {
            Debug.LogError("Raycast failed after " + maxAttempts + " attempts.");
        }

        onListChanged?.Invoke();
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
