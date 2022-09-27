#define _DEBUG_MODE_
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using MySql.Data.MySqlClient;

#if _DEBUG_MODE_
using MySql;
#endif

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
    private TextAsset _connectionText;
    private TextAsset _selectText;
    private string _connectionString;
    private string _selectString;

    private void Start()
    {
        _logInUIManager = GetComponentInParent<LogInUIManager>();

        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");

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
#if _DEBUG_MODE_
        
        if(!MySqlSetting.IsThereValue(MySqlSetting.EAccountColumnType.ID, _idInput.text))
        {
            _idErrorText.SetActive(true);
            _passwordErrorText.SetActive(false);
            return;
        }

        _idErrorText.SetActive(false);
        if(MySqlSetting.CheckValueByBase(MySqlSetting.EAccountColumnType.ID, _idInput.text,
            MySqlSetting.EAccountColumnType.Password, _passwordInput.text))
        {
            _passwordErrorText.SetActive(false);
            PlayerPrefs.SetString("ID", _idInput.text);
            LoadWaitingRoom();
        }
        else
        {
            _passwordErrorText.SetActive(true);
        }
#else
        _selectString = _selectText.text + $" where binary ID= '{_idInput.text}';";

        using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
        {
            sqlConnection.Open();
            MySqlCommand readCommand = new MySqlCommand(_selectString, sqlConnection);
            MySqlDataReader dataReader = readCommand.ExecuteReader();
            if (dataReader.Read())
            {
                _idErrorText.SetActive(false);

                if (dataReader["Password"].ToString() == _passwordInput.text)
                {
                    _passwordErrorText.SetActive(false);

                    PlayerPrefs.SetString("ID", dataReader["ID"].ToString());

                    sqlConnection.Close();
                    LoadWaitingRoom();
                    return;
                }
                else
                {
                    _passwordErrorText.SetActive(true);
                }
            }
            else
            {
                _idErrorText.SetActive(true);
            }
            sqlConnection.Close();
        }
#endif
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
