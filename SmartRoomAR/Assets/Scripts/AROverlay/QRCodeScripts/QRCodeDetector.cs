
using ZXing;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;

using Unity.Collections;

public class QRCodeDetector : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager m_CameraManager;
    [SerializeField]
    private string _lastResult;

    private Texture2D _cameraImageTexture;
    private NativeArray<byte> _buffer;
    private bool _bufferIsInitialized = false;

    public delegate void QRCodeDetectedDelegate(string qrCodeData, Vector2? screenPosition = null);
    public event QRCodeDetectedDelegate OnQRCodeDetected;

    

    private IBarcodeReader _barcodeReader = new BarcodeReader
    {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions
        {
            PossibleFormats = new System.Collections.Generic.List<ZXing.BarcodeFormat>
            {
                ZXing.BarcodeFormat.QR_CODE //Checks for QR codes only.
            },

            TryHarder = false
        }
    };

    
    private float _frameProcessInterval = 1.0f; //Scans an frame every 1 sec
    private float _lastFrameProcessedTime = 0.0f;


    public void SetLastResultNull() {
        _lastResult = null;   
    }

    private void OnEnable()
    {
        m_CameraManager.frameReceived += OnCameraFrameReceived;
    }
    private void OnDisable()
    {
        m_CameraManager.frameReceived -= OnCameraFrameReceived;
        if (_bufferIsInitialized)
        {
            _buffer.Dispose();
            _bufferIsInitialized = false;
        }
        if (_cameraImageTexture != null)
        {
            Destroy(_cameraImageTexture);
            _cameraImageTexture = null;
        }
    }



    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (Time.time - _lastFrameProcessedTime < _frameProcessInterval)
            return;

        _lastFrameProcessedTime = Time.time;

        

        // Acquire an XRCpuImage
        if (!m_CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;


        // Set up our conversion paramameters
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

        try
        {
            // Allocate a buffer to store the image.
            if (!_bufferIsInitialized || _buffer.Length != size)
            {
                if (_bufferIsInitialized) _buffer.Dispose();
                _buffer = new NativeArray<byte>(size, Allocator.Temp);
                _bufferIsInitialized = true;
            }

            //Extract the data from the image
            image.Convert(conversionParams, _buffer);


            //We now make the Texture2D that we need for the decode section.
            if (_cameraImageTexture == null || _cameraImageTexture.width != conversionParams.outputDimensions.x || _cameraImageTexture.height != conversionParams.outputDimensions.y)
            {

                Destroy(_cameraImageTexture);

                _cameraImageTexture = new Texture2D(
                    conversionParams.outputDimensions.x,
                    conversionParams.outputDimensions.y,
                    conversionParams.outputFormat,
                    false);
            }

            //Load raw pixel data into the Texture2D
            _cameraImageTexture.LoadRawTextureData(_buffer);
            _cameraImageTexture.Apply();


            Result _result = _barcodeReader.Decode(
               _cameraImageTexture.GetPixels32(),
               _cameraImageTexture.width,
               _cameraImageTexture.height);

            Destroy(_cameraImageTexture);


            if (_result != null && _result.Text != _lastResult)
            {

                _lastResult = _result.Text;

                Vector2 _qrCodeScreenPos = CalculateQRCodeScreenPosition(_result, image.width, image.height);

                Debug.Log(_qrCodeScreenPos);
                Debug.Log(_lastResult);
                // Send the result to other scripts

                OnQRCodeDetected?.Invoke(_lastResult, _qrCodeScreenPos);


            }
        }
        finally
        {
            image.Dispose();
            if (_bufferIsInitialized)
            {
                _buffer.Dispose();
                _bufferIsInitialized = false;
            }
            if (_cameraImageTexture != null)
            {
                Destroy(_cameraImageTexture);
                _cameraImageTexture = null;
            }

        }

    }

    private Vector2 CalculateQRCodeScreenPosition(Result result, int imageWidth, int imageHeight)
    {

        
        var points = result.ResultPoints;
        float x = 0f;
        float y = 0f;

        foreach (var point in points)
        {
            x += point.X;
            y += point.Y;
        }

        x /= points.Length;
        y /= points.Length;

        // Convert to screen space
        x = x / imageWidth * Screen.width;
        y = (1 - y / imageHeight) * Screen.height; // Inverting y as screen coordinates are flipped

        return new Vector2(x, y);
    }

}





