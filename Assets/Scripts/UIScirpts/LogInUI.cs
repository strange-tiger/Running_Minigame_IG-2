using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using MySql.Data.MySqlClient;

public class LogInUI : MonoBehaviour
{
    public LogInUIManager LogInUIManager;

    public Button LogInBtn;
    public Button SignInBtn;
    public Button FindButton;
    public Button QuitBtn;
    
    public InputField IDInput;
    public InputField PasswordInput;

    private GameObject _idErrorText;
    private GameObject _pwErrorText;
    private TextAsset _connectionText;
    private TextAsset _selectText;
    private string _connectionString;
    private string _selectString;

    private void Start()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");

        int loginChildIndex = IDInput.transform.childCount - 1;

        _idErrorText = IDInput.transform.GetChild(loginChildIndex).gameObject;
        _pwErrorText = PasswordInput.transform.GetChild(loginChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText.SetActive(false);

        // PlayerPrefs.DeleteAll();

        if (PlayerPrefs.GetString("ID") != null)
            IDInput.text = PlayerPrefs.GetString("ID");
    }
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        FindButton.onClick.AddListener(LoadFind);
        QuitBtn.onClick.AddListener(LoadQuit);

        _idErrorText?.SetActive(false);
        _pwErrorText?.SetActive(false);
    }
    

    public void LoadLogIn()
    {

            _selectString = _selectText.text + $" where ID= '{IDInput.text}';";

        using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
        {
            sqlConnection.Open();
            MySqlCommand readCommand = new MySqlCommand(_selectString, sqlConnection);
            MySqlDataReader dataReader = readCommand.ExecuteReader();
            if(dataReader.Read())
            {
                  _idErrorText.SetActive(false);
                
                if(dataReader["Password"].ToString() == PasswordInput.text)
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
        SceneManager.LoadScene(1);
    }

    public void LoadSignIn() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SignIn); 
    public void LoadFind() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.Find); 
    public void LoadQuit() => LogInUIManager.LoadQuitCheck();

    private void OnDisable()
    {
        IDInput.text = "";
        PasswordInput.text = "";
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        SignInBtn.onClick.RemoveListener(LoadSignIn);
        FindButton.onClick.RemoveListener(LoadFind);
        QuitBtn.onClick.RemoveListener(LoadQuit);
    }
}
