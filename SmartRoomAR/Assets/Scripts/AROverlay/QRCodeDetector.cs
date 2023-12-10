using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.Mobile;

public class QRReader : MonoBehaviour
{
    private WebCamTexture camTexture;
    private Rect screenRect;
    private BarcodeScanner scanner;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // Använd den första kameran eller välj den du vill använda
        camTexture = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        camTexture.Play();

        // Skapa en rektangel som täcker hela skärmen
        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        // Skapa en instans av BarcodeScanner
        scanner = new BarcodeScanner();
        scanner.Camera = camTexture;
    }

    void Update()
    {
        // Läs av kamerabilden
        if (camTexture != null && camTexture.isPlaying)
        {
            // Fånga bildrutan från kameran
            Color32[] data = camTexture.GetPixels32();

            // Starta skanningen
            var result = scanner.Scan(data, camTexture.width, camTexture.height);

            // Om en QR-kod hittades
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                Debug.Log("QR Code detected: " + result.Text);
            }
        }
    }
}
