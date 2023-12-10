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

        // Anv�nd den f�rsta kameran eller v�lj den du vill anv�nda
        camTexture = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        camTexture.Play();

        // Skapa en rektangel som t�cker hela sk�rmen
        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        // Skapa en instans av BarcodeScanner
        scanner = new BarcodeScanner();
        scanner.Camera = camTexture;
    }

    void Update()
    {
        // L�s av kamerabilden
        if (camTexture != null && camTexture.isPlaying)
        {
            // F�nga bildrutan fr�n kameran
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
