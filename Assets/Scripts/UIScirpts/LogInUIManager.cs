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

        for (int i = 0; i < uiNumber; ++i)
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

    // ELogInUIIndex를 매개변수로 받아, ui 오브젝트를 모두 비활성화한 후 인덱스에 해당하는 ui 오브젝트를 Quit을 제외하고 활성화한다.
    public void LoadUI(ELogInUIIndex ui)
    {
        ShutUI();
        _ui[(int)ui].SetActive(true);
    }

    // Quit ui 오브젝트를 활성화한다.
    public void LoadQuit()
    {
        _ui[(int)ELogInUIIndex.Quit].SetActive(true);
    }
}
