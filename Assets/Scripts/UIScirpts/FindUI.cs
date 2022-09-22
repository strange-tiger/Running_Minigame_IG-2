using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindUI : MonoBehaviour
{
    public Button LogInBtn;
    public Button SignInBtn;
    public Button Id_EnterBtn;
    public Button Pw_EnterBtn;

    public InputField Id_EmailInput;
    public InputField Id_Output;
    public InputField Pw_EmailInput;
    public InputField Pw_IDInput;
    public InputField Pw_Output;

    private GameObject _id_EmailErrorText;
    private GameObject _pw_EmailErrorText;
    private GameObject _pw_IDErrorText;
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        Id_EnterBtn.onClick.AddListener(FindID);
        Pw_EnterBtn.onClick.AddListener(FindPW);
        
        _id_EmailErrorText = Id_EmailInput.transform.GetChild(2).gameObject;
        _id_EmailErrorText.SetActive(false);
        _pw_EmailErrorText = Pw_EmailInput.transform.GetChild(2).gameObject;
        _pw_EmailErrorText.SetActive(false);
        _pw_IDErrorText = Pw_IDInput.transform.GetChild(2).gameObject;
        _pw_IDErrorText.SetActive(false);
    }

    public void LoadLogIn() => UIManager.Instance.LoadUI(EUIIndex.LogIn);
    public void LoadSignIn() => UIManager.Instance.LoadUI(EUIIndex.SignIn);

    public void FindID()
    {
        
    }

    public void FindPW()
    {

    }

    private void OnDisable()
    {
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        SignInBtn.onClick.RemoveListener(LoadSignIn);
        Id_EnterBtn.onClick.RemoveListener(FindID);
        Pw_EnterBtn.onClick.RemoveListener(FindPW);
    }
}
