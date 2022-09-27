using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingUI : MonoBehaviour
{
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
