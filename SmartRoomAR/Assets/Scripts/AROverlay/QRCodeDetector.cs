using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRReader : MonoBehaviour
{
    private WebCamTexture camTexture;
    private Rect screenRect;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // Använd den första kameran eller välj den du vill använda
        camTexture = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        camTexture.Play();

        // Skapa en rektangel som täcker hela skärmen
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    void Update()
    {
        // Läs av kamerabilden
        if (camTexture != null && camTexture.isPlaying)
        {
            // Fånga bildrutan från kameran
            Color32[] data = camTexture.GetPixels32();

            // Skapa en avkodare
            IBarcodeReader barcodeReader = new BarcodeReader();

            // Få avkodningsresultatet
            Result result = barcodeReader.Decode(data, camTexture.width, camTexture.height);

            // Om en QR-kod hittades
            if (result != null)
            {
                Debug.Log("QR Code detected: " + result.Text);
            }
        }
    }
}
