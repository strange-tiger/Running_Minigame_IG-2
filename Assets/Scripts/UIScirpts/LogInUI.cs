using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using MySql.Data.MySqlClient;

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
    private GameObject _pwErrorText;
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
        _pwErrorText = _passwordInput.transform.GetChild(loginChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText.SetActive(false);

        if (PlayerPrefs.GetString("ID") != null)
            _idInput.text = PlayerPrefs.GetString("ID");
    }
    private void OnEnable()
    {
        _logInButton.onClick.AddListener(LoadLogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _findButton.onClick.AddListener(LoadFind);
        _quitButton.onClick.AddListener(LoadQuit);

        _idErrorText?.SetActive(false);
        _pwErrorText?.SetActive(false);
    }
    

    public void LoadLogIn()
    {
        _selectString = _selectText.text + $" where binary ID= '{_idInput.text}';";

        using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
        {
            sqlConnection.Open();
            MySqlCommand readCommand = new MySqlCommand(_selectString, sqlConnection);
            MySqlDataReader dataReader = readCommand.ExecuteReader();
            if(dataReader.Read())
            {
                  _idErrorText.SetActive(false);
                
                if(dataReader["Password"].ToString() == _passwordInput.text)
                {
                    _pwErrorText.SetActive(false);

                    PlayerPrefs.SetString("ID", dataReader["ID"].ToString());

                    sqlConnection.Close();
                    LoadWaitingRoom();
                    return;
                }
                else
                {
                    _pwErrorText.SetActive(true);
                }
            }
            else
            {
                _idErrorText.SetActive(true);
            }
            sqlConnection.Close();
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
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(LoadQuit);
    }
}
