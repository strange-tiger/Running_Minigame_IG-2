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
        LoadUI(ELogInUIIndex.LogIn);
    }

    private void ShutUI()
    {
        foreach (GameObject ui in _ui)
        {
            ui.SetActive(false);
        }
    }

    public void LoadUI(ELogInUIIndex ui)
    {
        ShutUI();
        _ui[(int)ui].SetActive(true);
    }

    public void LoadQuit()
    {
        _ui[(int)ELogInUIIndex.Quit].SetActive(true);
    }
}
