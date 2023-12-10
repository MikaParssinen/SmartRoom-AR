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

        // Anv�nd den f�rsta kameran eller v�lj den du vill anv�nda
        camTexture = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        camTexture.Play();

        // Skapa en rektangel som t�cker hela sk�rmen
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    void Update()
    {
        // L�s av kamerabilden
        if (camTexture != null && camTexture.isPlaying)
        {
            // F�nga bildrutan fr�n kameran
            Color32[] data = camTexture.GetPixels32();

            // Skapa en avkodare
            IBarcodeReader barcodeReader = new BarcodeReader();

            // F� avkodningsresultatet
            Result result = barcodeReader.Decode(data, camTexture.width, camTexture.height);

            // Om en QR-kod hittades
            if (result != null)
            {
                Debug.Log("QR Code detected: " + result.Text);
            }
        }
    }
}
