using UnityEngine;

public class TouchToRemove : MonoBehaviour
{
    [SerializeField] private Camera arCamera; // Assign this in the Unity Editor
    private BuildARFromQRCode arManager;

    void Start()
    {
        arManager = FindObjectOfType<BuildARFromQRCode>();
        if (arManager == null)
        {
            Debug.LogError("AR Manager not found in the scene.");
        }

        if (arCamera == null)
        {
            Debug.LogError("AR Camera is not assigned.");
        }
    }

    void Update()
    {
        if (arCamera == null)
        {
            Debug.Log("Arcamera is null");
           
        }
        if( arManager == null)
        {
            Debug.Log("arManager is null");
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                arManager.RemoveSpecificARObject(hitObject);
            }
        }
    }
}