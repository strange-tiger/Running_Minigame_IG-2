using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Asset.MySql;

public class LogInUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button _logInButton;
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _findButton;
    [SerializeField] private Button _quitButton;

    [Header("Input Field")]
    [SerializeField] private InputField _idInput;
    [SerializeField] private InputField _passwordInput;

    private LogInUIManager _logInUIManager;

    private GameObject _idErrorText;
    private GameObject _passwordErrorText;

    private void Start()
    {
        _logInUIManager = GetComponentInParent<LogInUIManager>();

        int loginChildIndex = _idInput.transform.childCount - 1;

        _idErrorText = _idInput.transform.GetChild(loginChildIndex).gameObject;
        _passwordErrorText = _passwordInput.transform.GetChild(loginChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _passwordErrorText.SetActive(false);

        if (PlayerPrefs.GetString("ID") != null)
            _idInput.text = PlayerPrefs.GetString("ID");
    }
    private void OnEnable()
    {
        _logInButton.onClick.AddListener(LogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _findButton.onClick.AddListener(LoadFind);
        _quitButton.onClick.AddListener(LoadQuit);

        _idErrorText?.SetActive(false);
        _passwordErrorText?.SetActive(false);
    }

    // 입력된 계정 정보를 계정 DB와 비교해 일치하면 ID를 PlayerPrefs에 저장하고 WaitingRoom 씬을 로드한다.
    public void LogIn()
    {
        if(!MySqlSetting.HasValue(EAccountColumnType.ID, _idInput.text))
        {
            _idErrorText.SetActive(true);
            _passwordErrorText.SetActive(false);
            return;
        }

        _idErrorText.SetActive(false);
        if(MySqlSetting.CheckValueByBase(EAccountColumnType.ID, _idInput.text,
            EAccountColumnType.Password, _passwordInput.text))
        {
            _passwordErrorText.SetActive(false);
            PlayerPrefs.SetString("ID", _idInput.text);
            LoadWaitingRoom();
        }
        else
        {
            _passwordErrorText.SetActive(true);
        }
    }

    private void LoadWaitingRoom()
    {
        GameManager.Instance.LogInInit();
        SceneManager.LoadScene(1);
    }

    public void LoadSignIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SignIn);
    public void LoadFind() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.Find);
    public void LoadQuit() => _logInUIManager.LoadQuit();

    private void OnDisable()
    {
        _idInput.text = "";
        _passwordInput.text = "";
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(LoadQuit);
    }
}
