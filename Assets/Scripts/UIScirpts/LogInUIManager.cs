using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogInUIManager : MonoBehaviour
{
    public enum ELogInUIIndex
    {
        LogIn,
        SignIn,
        Find,
        Quit,
        Max
    }

    public GameObject LogInUI;
    public GameObject SignInUI;
    public GameObject FindUI;
    public GameObject QuitUI;

    public void LoadUI(ELogInUIIndex ui) 
    {
        LogInUI.SetActive(false);
        SignInUI.SetActive(false);
        FindUI.SetActive(false);
        QuitUI.SetActive(false);

        switch(ui)
        {
            case ELogInUIIndex.LogIn:
                LogInUI.SetActive(true);
                break;
            case ELogInUIIndex.SignIn:
                SignInUI.SetActive(true);
                break;
            case ELogInUIIndex.Find:
                FindUI.SetActive(true);
                break;
            default:
                Debug.Assert(ui >= ELogInUIIndex.Quit, "Error: No UI Exists");
                break;
        }
    }

    public void LoadQuitCheck()
    {
        QuitUI.SetActive(true);
    }
}
