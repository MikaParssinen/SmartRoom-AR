using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum APIModalState
{
    Entry, Loading, Success, Failed
}

public class APIModalStateHandler : MonoBehaviour
{


    [SerializeField] private TMP_InputField usernameInputField, passwordInputField, apiKeyInputField;
    [SerializeField] private Button confirmButton, closeButton, tryAgainButton;
    [SerializeField] private TextMeshProUGUI provideApiKeyText, failedValidationText, successValidationText, loadingText;
    [SerializeField] private Image failedAnimationImage, successAnimationImage;
    [SerializeField] private GameObject loadingAnimation;

    public APIAuth apiAuth;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        confirmButton.onClick.AddListener(ConfirmButtonPressed);
        closeButton.onClick.AddListener(CloseButtonPressed);
        tryAgainButton.onClick.AddListener(TryAgainButtonPressed);
    }

    // Start is called before the first frame update
    void Start()
    {
        //If playerpref token exists and goes through validation, set state to success
        if(PlayerPrefs.HasKey("token"))
        {
            bool result = apiAuth.Validate(PlayerPrefs.GetString("token"));
            if(result)
            {
                SetState(APIModalState.Success);
            }
            else
            {
                SetState(APIModalState.Entry);
            }

        }
        else
        {
            SetState(APIModalState.Entry);
        }
        SetState(APIModalState.Entry);
        StartTransition();
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void SetState (APIModalState state)
    {
        switch (state)
        {
            case APIModalState.Entry:
                //True States
                provideApiKeyText.gameObject.SetActive(true);
                usernameInputField.gameObject.SetActive(true);
                passwordInputField.gameObject.SetActive(true);
                apiKeyInputField.gameObject.SetActive(true);
                confirmButton.gameObject.SetActive(true);
                //False States
                closeButton.gameObject.SetActive(false);
                tryAgainButton.gameObject.SetActive(false);
                failedValidationText.gameObject.SetActive(false);
                successValidationText.gameObject.SetActive(false);
                loadingText.gameObject.SetActive(false);
                failedAnimationImage.gameObject.SetActive(false);
                successAnimationImage.gameObject.SetActive(false);
                loadingAnimation.gameObject.SetActive(false);
                break;
            case APIModalState.Loading:
                //True States
                loadingText.gameObject.SetActive(true);
                loadingAnimation.gameObject.SetActive(true);
                //False States
                usernameInputField.gameObject.SetActive(false);
                passwordInputField.gameObject.SetActive(false);
                apiKeyInputField.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(false);
                closeButton.gameObject.SetActive(false);
                tryAgainButton.gameObject.SetActive(false);
                provideApiKeyText.gameObject.SetActive(false);
                failedValidationText.gameObject.SetActive(false);
                successValidationText.gameObject.SetActive(false);
                failedAnimationImage.gameObject.SetActive(false);
                successAnimationImage.gameObject.SetActive(false);
                break;
            case APIModalState.Success:
                //True States
                closeButton.gameObject.SetActive(true);
                successValidationText.gameObject.SetActive(true);
                successAnimationImage.gameObject.SetActive(true);
                //False States
                usernameInputField.gameObject.SetActive(false);
                passwordInputField.gameObject.SetActive(false);
                apiKeyInputField.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(false);     
                tryAgainButton.gameObject.SetActive(false);
                provideApiKeyText.gameObject.SetActive(false);
                failedValidationText.gameObject.SetActive(false);         
                loadingText.gameObject.SetActive(false);
                failedAnimationImage.gameObject.SetActive(false);              
                loadingAnimation.gameObject.SetActive(false);
                break;
            case APIModalState.Failed:
                //True States
                failedValidationText.gameObject.SetActive(true);
                failedAnimationImage.gameObject.SetActive(true);
                tryAgainButton.gameObject.SetActive(true);
                //False States
                usernameInputField.gameObject.SetActive(false);
                passwordInputField.gameObject.SetActive(false);
                apiKeyInputField.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(false);
                closeButton.gameObject.SetActive(false);            
                provideApiKeyText.gameObject.SetActive(false);               
                successValidationText.gameObject.SetActive(false);
                loadingText.gameObject.SetActive(false);              
                successAnimationImage.gameObject.SetActive(false);
                loadingAnimation.gameObject.SetActive(false);
                break;
        }
    }

    public void ConfirmButtonPressed()
    {
        SetState(APIModalState.Loading);
        apiAuth.Authenticate(usernameInputField.text, passwordInputField.text);
        apiAuth.OnAuthenticationComplete += HandleAuthResult;
        
    }

    private void HandleAuthResult(bool result)
    {
        if (result)
        {
            SetState(APIModalState.Success);
        }
        else
        {
            SetState(APIModalState.Failed);
        }
    }

    public void TryAgainButtonPressed()
    {
        SetState(APIModalState.Entry);
    }

    public void CloseButtonPressed()
    {
        //Transition out then destroy
        EndTransition();
        StartCoroutine(WaitAndDeactivate(2f));
        
    }

    private IEnumerator WaitAndSetState(APIModalState newState, float delay)
    {
    yield return new WaitForSeconds(delay);
    SetState(newState);
    }

    private IEnumerator WaitAndDeactivate(float delay)
    {
    yield return new WaitForSeconds(delay);
    gameObject.SetActive(false);
    }


    private void StartTransition(){

         RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -2000);

        // Move the panel to the center of the screen
        LeanTween.moveY(rectTransform, 7, 1.5f).setEase(LeanTweenType.easeOutBack);
    }

    private void EndTransition(){
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 7);

        
        LeanTween.moveY(rectTransform, -2000, 1.5f).setEase(LeanTweenType.easeOutBack);
    }
}
