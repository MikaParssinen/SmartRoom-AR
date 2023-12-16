
using ZXing;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;

public class QRCodeDetector : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager m_CameraManager;
    [SerializeField]
    private string _lastResult;

    private Texture2D _cameraImageTexture;

    private IBarcodeReader _barcodeReader = new BarcodeReader
    {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions
        {
            TryHarder = false
        }
    };

    private Result _result;



    private void OnEnable()
    {
        m_CameraManager.frameReceived += OnCameraFrameReceived;
    }
    private void OnDisable()
    {
        m_CameraManager.frameReceived -= OnCameraFrameReceived;
    }



    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
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

        // See how many bytes we need to store the image  
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        //Extract the data from the image
        image.Convert(conversionParams, buffer);

        //Now we dispose the XRCpuImage because we have everything we need in the buffer.
        image.Dispose();


        //We now make the Texture2D that we need for the decode section.
        if (_cameraImageTexture == null || _cameraImageTexture.width != conversionParams.outputDimensions.x || _cameraImageTexture.height != conversionParams.outputDimensions.y)
        {
            _cameraImageTexture = new Texture2D(
                conversionParams.outputDimensions.x,
                conversionParams.outputDimensions.y,
                conversionParams.outputFormat,
                false);
        }


        _cameraImageTexture.LoadRawTextureData(buffer);
        _cameraImageTexture.Apply();

        buffer.Dispose();

        _result = _barcodeReader.Decode(
            _cameraImageTexture.GetPixels32(),
            _cameraImageTexture.width,
            _cameraImageTexture.height);

        Destroy(_cameraImageTexture);
        
        if (_result != null)
        {
            _lastResult = _result.Text + " " + _result.BarcodeFormat;
            Debug.Log(_lastResult);
        }



    }
}





