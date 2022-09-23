using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;

public class LogInUI : MonoBehaviour
{
    public LogInUIManager LogInUIManager;

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
    private bool _isExistId;
    private bool _canLogIn;

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
    

    public DataSet GetUserData()
    {
        DataSet _dataSet = new DataSet();

        _selectString = _selectText.text + $" where ID= '{IDInput.text}';";

        using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            _sqlConnection.Open();
            MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(_selectString, _sqlConnection);
           
            _dataAdapter.Fill(_dataSet);
        }
        return _dataSet;
    }
    public void LoadLogIn()
    {
        DataSet _logInDataSet;
        _logInDataSet = GetUserData();
      
        if(_logInDataSet.Tables[0].Rows.Count == 0)
        {
            _idErrorText.SetActive(true);
            return;
        }
        _idErrorText.SetActive(false);

        foreach(DataRow _dataRow in _logInDataSet.Tables[0].Rows)
        {
            if(_dataRow["Password"].ToString() == PWInput.text)
            {
                _pwErrorText.SetActive(false);
            }
            else
            {
                _pwErrorText.SetActive(true);
            }
        }
    }
    
    public void LoadSignIn() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SignIn); 
    public void LoadFind() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.Find); 
    public void LoadQuit() => LogInUIManager.LoadQuitCheck();

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
