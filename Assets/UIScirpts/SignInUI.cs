using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInUI : MonoBehaviour
{
    public Button CreateAccountBtn;
    public Button LogInBtn;
    public Button FindBtn;
    public Button DoubleCheckBtn;

    public InputField IDInput;
    public InputField PWInput;
    public InputField PWCheckInput;
    public InputField EmailInput;

    private void OnEnable()
    {
        CreateAccountBtn.onClick.AddListener(CreateAccount);
        LogInBtn.onClick.AddListener(LoadLogIn);
        FindBtn.onClick.AddListener(LoadFind);
        DoubleCheckBtn.onClick.AddListener(DoubleCheck);
    }

    public void CreateAccount()
    {

    }
    
    public void LoadLogIn() => UIManager.Instance.LoadUI(EUIIndex.LogIn);
    public void LoadFind() => UIManager.Instance.LoadUI(EUIIndex.Find);
    
    public void DoubleCheck()
    {

    }

    private void OnDisable()
    {
        CreateAccountBtn.onClick.RemoveListener(CreateAccount);
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        FindBtn.onClick.RemoveListener(LoadFind);
        DoubleCheckBtn.onClick.RemoveListener(DoubleCheck);
    }
}
