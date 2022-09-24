using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingUI : MonoBehaviour
{
    public enum EWaitingUIIndex
    {
        Quit,
        LogOut,
        Max
    }

    public GameObject LogOutUI;
    public GameObject QuitUI;

    public Button StartBtn;
    public Button QuitBtn;
    public Button LogOutBtn;

    private void Awake()
    {
        StartBtn.onClick.AddListener(StartGame);
        QuitBtn.onClick.AddListener(LoadQuit);
        LogOutBtn.onClick.AddListener(LoadLogOut);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadUI(EWaitingUIIndex ui)
    {
        LogOutUI.SetActive(false);
        QuitUI.SetActive(false);

        switch (ui)
        {
            case EWaitingUIIndex.Quit:
                QuitUI.SetActive(true);
                Debug.Log("Quit");
                break;
            case EWaitingUIIndex.LogOut:
                LogOutUI.SetActive(true);
                break;
            default:
                Debug.Assert(ui >= EWaitingUIIndex.Max, "Error: No UI Exists");
                break;
        }
    }

    public void LoadQuit() => LoadUI(EWaitingUIIndex.Quit);
    public void LoadLogOut() => LoadUI(EWaitingUIIndex.LogOut);

    private void OnDisable()
    {
        StartBtn.onClick.RemoveListener(StartGame);
        QuitBtn.onClick.RemoveListener(LoadQuit);
        LogOutBtn.onClick.RemoveListener(LoadLogOut);
    }
}
