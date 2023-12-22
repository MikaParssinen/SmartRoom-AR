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

    private void PlaceObjectInAR(string qrCodeData, Vector2 screenPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPosition, hits, TrackableType.Planes))
        {
            Debug.Log("Trying to calculate");
            Pose hitPose = hits[0].pose;
            objectToPlace.transform.position = hitPose.position;
            objectToPlace.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.current.transform.forward, Vector3.up));
        }
    }





}
