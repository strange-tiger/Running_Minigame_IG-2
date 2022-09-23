using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;

public class LogInUI : MonoBehaviour
{
    public Button LogInBtn;
    public Button SignInBtn;
    public Button FindBtn;
    public Button QuitBtn;
    
    public InputField IDInput;
    public InputField PWInput;

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

        int _loginChildIndex = IDInput.transform.childCount - 1;

        _idErrorText = IDInput.transform.GetChild(_loginChildIndex).gameObject;
        _pwErrorText = PWInput.transform.GetChild(_loginChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText.SetActive(false);
    }
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        FindBtn.onClick.AddListener(LoadFind);
        QuitBtn.onClick.AddListener(LoadQuit);


        _isExistId = false;

        _idErrorText?.SetActive(false);
        _pwErrorText?.SetActive(false);
    }
    

    public void LoadLogIn()
    {

            _selectString = _selectText.text + $" where ID= '{IDInput.text}';";

        using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            _sqlConnection.Open();
            MySqlCommand _readCommand = new MySqlCommand(_selectString, _sqlConnection);
            MySqlDataReader _dataReader = _readCommand.ExecuteReader();
            if(_dataReader.Read())
            {
                  _idErrorText.SetActive(false);
                
                if(_dataReader["Password"].ToString() == PWInput.text)
                {
                    _pwErrorText.SetActive(false);
                }
                else
                {
                    _pwErrorText.SetActive(true);
                }
            }
            else
            {
                _idErrorTExt.SetActive(true);
            }
            _sqlConnection.Close();

        }
    }
    
    public void LoadSignIn() => UIManager.Instance.LoadUI(EUIIndex.SignIn); 
    public void LoadFind() => UIManager.Instance.LoadUI(EUIIndex.Find); 
    public void LoadQuit() => UIManager.Instance.LoadQuitCheck();

    private void OnDisable()
    {
        IDInput.text = "";
        PWInput.text = "";
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        SignInBtn.onClick.RemoveListener(LoadSignIn);
        FindBtn.onClick.RemoveListener(LoadFind);
        QuitBtn.onClick.RemoveListener(LoadQuit);
    }
}
