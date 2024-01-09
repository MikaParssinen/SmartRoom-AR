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

    private GameObject instantiatedObject; // Reference to the instantiated object


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
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast((Vector2)screenPosition, hits, TrackableType.Planes))
        {
            
            Pose hitPose = hits[0].pose;
            Debug.Log("Trying to Instantiate");
            // Instantiate the prefab at the hit position and rotation
            instantiatedObject = Instantiate(objectToPlace, hitPose.position, Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.current.transform.forward, Vector3.up)));
        }
    }

    public void UpdateARObjectWithData(string title, string content)
    {

        //title = "Termostat";
        //content = "1 degree, 2 users, 6 micophones";

        if (instantiatedObject != null)
        {
            // Try getting the InfoPanel component
            InfoPanel arObjectController = instantiatedObject.GetComponent<InfoPanel>();

            // If not found, try searching in child objects
            if (arObjectController == null)
            {
                arObjectController = instantiatedObject.GetComponentInChildren<InfoPanel>();
                if (arObjectController == null)
                {
                    Debug.LogError("InfoPanel component not found on the instantiated object or its children.");
                }
            }

            if (arObjectController != null)
            {
                Debug.Log($"Updating InfoPanel with Title: {title}, Content: {content}");
                arObjectController.ActivatePanel(title, content, true);
            }
        }
        else
        {
            Debug.LogError("No instantiated object to update.");
        }
    }


}
