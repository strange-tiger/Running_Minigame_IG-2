using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInUI : MonoBehaviour
{
    public Button LogInBtn;
    public Button SignInBtn;
    public Button FindBtn;
    public Button QuitBtn;
    
    public InputField IDInput;
    public InputField PWInput;

    private GameObject _idErrorText;
    private GameObject _pwErrorText;
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        FindBtn.onClick.AddListener(LoadFind);
        QuitBtn.onClick.AddListener(LoadQuit);

        _idErrorText = IDInput.transform.GetChild(2).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText = PWInput.transform.GetChild(2).gameObject;
        _pwErrorText.SetActive(false);
    }

    public void LoadLogIn()
    {

    }
    
    public void LoadSignIn() => UIManager.Instance.LoadUI(EUIIndex.SignIn); 
    public void LoadFind() => UIManager.Instance.LoadUI(EUIIndex.Find); 
    public void LoadQuit() => UIManager.Instance.LoadQuitCheck();

    private void OnDisable()
    {
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        SignInBtn.onClick.RemoveListener(LoadSignIn);
        FindBtn.onClick.RemoveListener(LoadFind);
        QuitBtn.onClick.RemoveListener(LoadQuit);
    }
}
