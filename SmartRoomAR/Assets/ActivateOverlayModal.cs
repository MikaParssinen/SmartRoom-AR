using UnityEngine;
using UnityEngine.UI;

public class ActivateOverlayModal : MonoBehaviour
{
    public GameObject panel;

    public void TogglePanelVisibility()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
