using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIIndex
{
    LogIn,
    SignIn,
    Find,
    Quit,
    Max
}

public class UIManager : SingletonBehaviour<UIManager>
{
    public GameObject LogInUI;
    public GameObject SignInUI;
    public GameObject FindUI;
    public GameObject QuitUI;

    public void LoadUI(EUIIndex ui) 
    {
        LogInUI.SetActive(false);
        SignInUI.SetActive(false);
        FindUI.SetActive(false);
        QuitUI.SetActive(false);

        switch(ui)
        {
            case EUIIndex.LogIn:
                LogInUI.SetActive(true);
                break;
            case EUIIndex.SignIn:
                SignInUI.SetActive(true);
                break;
            case EUIIndex.Find:
                FindUI.SetActive(true);
                break;
            default:
                Debug.Assert(ui >= EUIIndex.Quit, "Error: No UI Exists");
                break;
        }
    }

    public void LoadQuitCheck()
    {
        QuitUI.SetActive(true);
    }
}
