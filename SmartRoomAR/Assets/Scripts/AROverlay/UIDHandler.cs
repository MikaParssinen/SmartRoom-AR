using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UIDHandler : MonoBehaviour
{
    // Reference to the QRCodeDetector script
    public QRCodeDetector qrCodeDetector;

    // Reference to the ApiManager script
    public APIManager apiManager;

    private void OnEnable()
    {
        // Subscribe to the QR code detection event
        qrCodeDetector.OnQRCodeDetected += HandleQRCodeDetected;
    }

    private void OnDisable()
    {
        // Unsubscribe from the QR code detection event
        qrCodeDetector.OnQRCodeDetected -= HandleQRCodeDetected;
    }

    private void HandleQRCodeDetected(string uid, Vector2? screenPosition)
    {
        string transformedUid = TransformUid(uid);

        // Send the transformed UID to the API manager for further processing
        apiManager.SendRequest(transformedUid);
    }

    private string TransformUid(string uid)
    {
        // Replace ":" with "%3A"
        Debug.Log("Transforming");
        return uid.Replace(":", "%3A");
    }
}