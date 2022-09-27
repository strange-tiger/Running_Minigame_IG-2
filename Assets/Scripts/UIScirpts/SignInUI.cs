using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class SignInUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button _createAccountButton;
    [SerializeField] private Button _logInButton;
    [SerializeField] private Button _findButton;
    [SerializeField] private Button _idDoubleCheckButton;
    [SerializeField] private Button _emailDoubleCheckButton;

    [Header("Input Field")]
    [SerializeField] private InputField _idInput;
    [SerializeField] private InputField _passwordInput;
    [SerializeField] private InputField _passwordCheckInput;
    [SerializeField] private InputField _emailInput;

    private LogInUIManager _logInUIManager;
    
    private GameObject _idErrorText;
    private GameObject _pwErrorText;
    private GameObject _emailErrorText;

    private TextAsset _connectionText;
    private TextAsset _insertAccountText;
    private TextAsset _insertScoreText;
    private TextAsset _selectText;
    private string _connectionString;
    private string _insertAccountString;
    private string _insertScoreString;
    private string _selectString;

    private bool _hasIdDoubleCheck;
    private bool _hasEmailDoubleCheck;
    private bool _isMatchPassword;
    private void Start()
    {
        _logInUIManager = GetComponentInParent<LogInUIManager>();
        
        _connectionText = Resources.Load<TextAsset>("Connection");
        _insertAccountText = Resources.Load<TextAsset>("InsertAccount");
        _insertScoreText = Resources.Load<TextAsset>("InsertRanking");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _selectString = _selectText.text + ";";

        int signInChildIndex = _idInput.transform.childCount - 1;

        _idErrorText = _idInput.transform.GetChild(signInChildIndex).gameObject;
        _pwErrorText = _passwordInput.transform.GetChild(signInChildIndex).gameObject;
        _emailErrorText = _emailInput.transform.GetChild(signInChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText.SetActive(false);
        _emailErrorText.SetActive(false);
    }
    private void OnEnable()
    {

        _createAccountButton.onClick.AddListener(CreateAccount);
        _logInButton.onClick.AddListener(LoadLogIn);
        _findButton.onClick.AddListener(LoadFind);
        _idDoubleCheckButton.onClick.AddListener(IdDoubleCheck);
        _emailDoubleCheckButton.onClick.AddListener(EmailDoubleCheck);

        _passwordInput.onValueChange.AddListener(CheckPassword);
        _passwordCheckInput.onValueChange.AddListener(CheckPassword);

        _idErrorText?.SetActive(false);
        _pwErrorText?.SetActive(false);
        _emailErrorText?.SetActive(false);

        _hasIdDoubleCheck = false;
        _hasEmailDoubleCheck = false;
        _isMatchPassword = false;
    }

    public void CreateAccount()
    {
        if(_hasIdDoubleCheck && _hasEmailDoubleCheck && _isMatchPassword)
        {
            _insertAccountString = _insertAccountText.text + $"('{_idInput.text}', '{_passwordInput.text}', '{_emailInput.text}');";
            _insertScoreString = _insertScoreText.text + $"('{_idInput.text}');";
            Debug.Log(_insertAccountString);
            Debug.Log(_insertScoreString);
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                MySqlCommand insertAccountCommand = new MySqlCommand(_insertAccountString, sqlConnection);
                sqlConnection.Open();
                insertAccountCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                MySqlCommand insertScoreCommand = new MySqlCommand(_insertScoreString, sqlConnection);
                sqlConnection.Open();
                insertScoreCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            _hasIdDoubleCheck = false;
            _hasEmailDoubleCheck = false;

            _idInput.text = "";
            _passwordInput.text = "";
            _passwordCheckInput.text = "";
            _emailInput.text = "";
        }
        else
        {
            Debug.Log("더블첵해야함");
        }
    }
    
    public void LoadLogIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LogIn);
    public void LoadFind() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.Find);

    public DataSet GetUserData()
    {
        DataSet dataSet = new DataSet();

        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(_selectString, connection);

            dataAdapter.Fill(dataSet);
        }
        return dataSet;
    }
    public void IdDoubleCheck()
    {
        
         DataSet dataSet;
         dataSet = GetUserData();
         Debug.Log(dataSet);
         bool check = false;

         foreach (DataRow dataRow in dataSet.Tables[0].Rows)
         {
             if (dataRow["ID"].ToString() == _idInput.text)
             {
                check = true;
                break;
             }
         }
         
        if(!check)
        {
            // Debug.Log("사용 가능");
            _hasIdDoubleCheck = true;
            _idErrorText.SetActive(false);
        }
        else
        {
            // Debug.Log("사용 불가능");
            _hasIdDoubleCheck = false;
            _idErrorText.SetActive(true);
        }
        
    }
    public void CheckPassword(string pw)
    {
        if (_passwordInput.text == _passwordCheckInput.text)
        {
            _isMatchPassword = true;
            _pwErrorText.SetActive(false);
        }
        else
        {
            _isMatchPassword = false;
            _pwErrorText.SetActive(true);
        }
    }
    public void EmailDoubleCheck()
    {

        DataSet dataSet;
        dataSet = GetUserData();
        Debug.Log(dataSet);
        bool check = false;

        foreach (DataRow dataRow in dataSet.Tables[0].Rows)
        {
            if (dataRow["Email"].ToString() == _idInput.text)
            {
                check = true;
                break;
            }
        }

        if (!check)
        {
            // Debug.Log("사용 가능");
            _hasEmailDoubleCheck = true;
            _idErrorText.SetActive(false);
        }
        else
        {
            // Debug.Log("사용 불가능");
            _hasEmailDoubleCheck = false;
            _idErrorText.SetActive(true);
        }

    }

    private void OnDisable()
    {
        _idInput.text = "";
        _passwordInput.text = "";
        _passwordCheckInput.text = "";
        _emailInput.text = "";

        _createAccountButton.onClick.RemoveListener(CreateAccount);
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findButton.onClick.RemoveListener(LoadFind);
        _idDoubleCheckButton.onClick.RemoveListener(IdDoubleCheck);
        _emailDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);

        _passwordInput.onValueChange.RemoveListener(CheckPassword);
        _passwordCheckInput.onValueChange.RemoveListener(CheckPassword);
    }
}
