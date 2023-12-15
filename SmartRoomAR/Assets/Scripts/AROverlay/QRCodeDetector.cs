
using ZXing;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using Unity.Collections.LowLevel.Unsafe;



public class QRCodeDetector : MonoBehaviour
{
    private ARCameraManager m_CameraManager;

    void Start()
    {

        // Get the ARCameraManager component
        if (m_CameraManager == null)
            m_CameraManager = GetComponent<ARCameraManager>();

        if (m_CameraManager == null)
        {
            Debug.LogError("ARCameraManager not found. Make sure the script is attached to a GameObject with ARCameraManager component.");
        }
        else
        {
            Debug.Log("Found an ARCameraManager");
        }



    }

    void Update()
    {
        // Acquire an XRCpuImage
        if (!m_CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        // Set up our conversion params
        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Convert the entire image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Output at full resolution
            outputDimensions = new Vector2Int(image.width, image.height),

            // Convert to RGBA format
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image)
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // Create a Texture2D to store the converted image
        var texture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);

        // Texture2D allows us write directly to the raw texture data as an optimization
        var rawTextureData = texture.GetRawTextureData<byte>();
        try
        {
            unsafe
            {
                // Synchronously convert to the desired TextureFormat
                image.Convert(
                    conversionParams,
                    new IntPtr(rawTextureData.GetUnsafePtr()),
                    rawTextureData.Length);
            }
        }
        finally
        {
            Debug.Log("Now we destroy!");
            // Dispose the XRCpuImage after we're finished to prevent any memory leaks
            image.Dispose();
        }

        // Apply the converted pixel data to our texture
        texture.Apply();
    }
}
