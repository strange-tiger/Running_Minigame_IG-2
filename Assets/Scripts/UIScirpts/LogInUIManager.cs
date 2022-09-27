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

    private GameObject[] _ui;
    private void Awake()
    {
        int uiNumber = transform.childCount;
        Debug.Assert((int)ELogInUIIndex.Max == uiNumber, "Index 추가 잊지 말고");
        _ui = new GameObject[uiNumber];
        
        for(int i = 0; i < uiNumber; ++i)
        {
            _ui[i] = transform.GetChild(i).gameObject;
        }
        LoadLogIn();
    }

    //public void LoadUI(ELogInUIIndex ui) 
    //{
    //    _ui[(int)ELogInUIIndex.LogIn].SetActive(false);
    //    _ui[(int)ELogInUIIndex.SignIn].SetActive(false);
    //    _ui[(int)ELogInUIIndex.Find].SetActive(false);
    //    _ui[(int)ELogInUIIndex.Quit].SetActive(false);

    //    switch(ui)
    //    {
    //        case ELogInUIIndex.LogIn:
    //            _logInUI.SetActive(true);
    //            break;
    //        case ELogInUIIndex.SignIn:
    //            _signInUI.SetActive(true);
    //            break;
    //        case ELogInUIIndex.Find:
    //            _findUI.SetActive(true);
    //            break;
    //        default:
    //            Debug.Assert(ui >= ELogInUIIndex.Quit, "Error: No UI Exists");
    //            break;
    //    }
    //}

    private void ShutUI()
    {
        foreach (GameObject ui in _ui)
        {
            ui.SetActive(false);
        }
    }

    public void LoadLogIn()
    {
        ShutUI();
        _ui[(int)ELogInUIIndex.LogIn].SetActive(true);
    }

    public void LoadSignIn()
    {
        ShutUI();
        _ui[(int)ELogInUIIndex.SignIn].SetActive(true);
    }

    public void LoadFind()
    {
        ShutUI();
        _ui[(int)ELogInUIIndex.Find].SetActive(true);
    }

    public void LoadQuit()
    {
        _ui[(int)ELogInUIIndex.Quit].SetActive(true);
    }
}
