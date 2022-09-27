using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingUI : MonoBehaviour
{
    //public enum EWaitingUIIndex
    //{
    //    Quit,
    //    LogOut,
    //    Max
    //}

    [Header("UI Panel")]
    [SerializeField] private GameObject _logOutUI;
    [SerializeField] private GameObject _quitUI;

    [Header("Button")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _logOutButton;

    private void Awake()
    {
        _startButton.onClick.AddListener(StartGame);
        _quitButton.onClick.AddListener(LoadQuit);
        _logOutButton.onClick.AddListener(LoadLogOut);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    //public void LoadUI(EWaitingUIIndex ui)
    //{
    //    _logOutUI.SetActive(false);
    //    _quitUI.SetActive(false);

    //    switch (ui)
    //    {
    //        case EWaitingUIIndex.Quit:
    //            _quitUI.SetActive(true);
    //            Debug.Log("Quit");
    //            break;
    //        case EWaitingUIIndex.LogOut:
    //            _logOutUI.SetActive(true);
    //            break;
    //        default:
    //            Debug.Assert(ui >= EWaitingUIIndex.Max, "Error: No UI Exists");
    //            break;
    //    }
    //}

    public void LoadQuit()
    {
        _logOutUI.SetActive(false);
        _quitUI.SetActive(true);
    }
    public void LoadLogOut()
    {
        _logOutUI.SetActive(true);
        _quitUI.SetActive(false);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _quitButton.onClick.RemoveListener(LoadQuit);
        _logOutButton.onClick.RemoveListener(LoadLogOut);
    }
}
