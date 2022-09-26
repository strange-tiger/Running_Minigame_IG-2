using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class SignInUI : MonoBehaviour
{
    public LogInUIManager LogInUIManager;

    public Button CreateAccountBtn;
    public Button LogInBtn;
    public Button FindButton;
    public Button DoubleCheckBtn;

    public InputField IDInput;
    public InputField PasswordInput;
    public InputField PWCheckInput;
    public InputField EmailInput;

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

    private bool _hasDoubleCheck;
    private bool _isMatchPassword;
    private void Start()
    {

        _connectionText = Resources.Load<TextAsset>("Connection");
        _insertAccountText = Resources.Load<TextAsset>("InsertAccount");
        _insertScoreText = Resources.Load<TextAsset>("InsertRanking");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _selectString = _selectText.text + ";";

        int signInChildIndex = IDInput.transform.childCount - 1;

        _idErrorText = IDInput.transform.GetChild(signInChildIndex).gameObject;
        _pwErrorText = PasswordInput.transform.GetChild(signInChildIndex).gameObject;
        _emailErrorText = EmailInput.transform.GetChild(signInChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText.SetActive(false);
        _emailErrorText.SetActive(false);
    }
    private void OnEnable()
    {

        CreateAccountBtn.onClick.AddListener(CreateAccount);
        LogInBtn.onClick.AddListener(LoadLogIn);
        FindButton.onClick.AddListener(LoadFind);
        DoubleCheckBtn.onClick.AddListener(DoubleCheck);

        PasswordInput.onValueChange.AddListener(CheckPassword);
        PWCheckInput.onValueChange.AddListener(CheckPassword);

        _idErrorText?.SetActive(false);
        _pwErrorText?.SetActive(false);
        _emailErrorText?.SetActive(false);

        _hasDoubleCheck = false;
        _isMatchPassword = false;
    }

    public void CreateAccount()
    {
        if(_hasDoubleCheck && _isMatchPassword)
        {
            _insertAccountString = _insertAccountText.text + $"('{IDInput.text}', '{PasswordInput.text}', '{EmailInput.text}');";
            _insertScoreString = _insertScoreText.text + $"('{IDInput.text}');";
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
            _hasDoubleCheck = false;

            IDInput.text = "";
            PasswordInput.text = "";
            PWCheckInput.text = "";
            EmailInput.text = "";
        }
        else
        {
            Debug.Log("더블첵해야함");
        }
    }
    
    public void LoadLogIn() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LogIn);
    public void LoadFind() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.Find);

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
    public void DoubleCheck()
    {
        
         DataSet dataSet;
         dataSet = GetUserData();
         Debug.Log(dataSet);
         bool check = false;

         foreach (DataRow dataRow in dataSet.Tables[0].Rows)
         {
             if (dataRow["ID"].ToString() == IDInput.text)
             {
                check = true;
                break;
             }
         }
         
        if(!check)
        {
            // Debug.Log("사용 가능");
            _hasDoubleCheck = true;
            _idErrorText.SetActive(false);
        }
        else
        {
            // Debug.Log("사용 불가능");
            _hasDoubleCheck = false;
            _idErrorText.SetActive(true);
        }
        
    }
    public void CheckPassword(string pw)
    {
        if (PasswordInput.text == PWCheckInput.text)
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

    private void OnDisable()
    {
        IDInput.text = "";
        PasswordInput.text = "";
        PWCheckInput.text = "";
        EmailInput.text = "";

        CreateAccountBtn.onClick.RemoveListener(CreateAccount);
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        FindButton.onClick.RemoveListener(LoadFind);
        DoubleCheckBtn.onClick.RemoveListener(DoubleCheck);

        PasswordInput.onValueChange.RemoveListener(CheckPassword);
        PWCheckInput.onValueChange.RemoveListener(CheckPassword);
    }
}
