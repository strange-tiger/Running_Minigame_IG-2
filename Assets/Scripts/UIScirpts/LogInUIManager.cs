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
        Debug.Assert((int)ELogInUIIndex.Max == uiNumber, "Index �߰� ���� ����");
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

    // ELogInUIIndex�� �Ű������� �޾�, ui ������Ʈ�� ��� ��Ȱ��ȭ�� �� �ε����� �ش��ϴ� ui ������Ʈ�� Quit�� �����ϰ� Ȱ��ȭ�Ѵ�.
    public void LoadUI(ELogInUIIndex ui)
    {
        ShutUI();
        _ui[(int)ui].SetActive(true);
    }

    // Quit ui ������Ʈ�� Ȱ��ȭ�Ѵ�.
    public void LoadQuit()
    {
        _ui[(int)ELogInUIIndex.Quit].SetActive(true);
    }
}
