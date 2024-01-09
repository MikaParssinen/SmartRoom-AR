using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BuildARFromQRCode : MonoBehaviour
{

    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject objectToPlace; // Your AR object
    [SerializeField] private QRCodeDetector qrCodeDetector; // Reference to your QR code detection script

    //private GameObject instantiatedObject; // Reference to the instantiated object

    private List<GameObject> instantiatedObjects = new List<GameObject>();


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

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast((Vector2)screenPosition, hits, TrackableType.Planes))
        {
            
            Pose hitPose = hits[0].pose;
            Debug.Log("Trying to Instantiate");
            // Instantiate the prefab at the hit position and rotation
            GameObject newObject = Instantiate(objectToPlace, hitPose.position, Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.current.transform.forward, Vector3.up)));
            instantiatedObjects.Add(newObject);

            // Set the name of the new object to the QR code data
            newObject.name = qrCodeData;

            Debug.Log("Instantiated new object at: " + hitPose.position);
        }
        else
        {
            Debug.Log("Raycast did not hit a plane for QR code: " + qrCodeData);
        }
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

    public void RemoveAllARObjects()
    {
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
    }


}
