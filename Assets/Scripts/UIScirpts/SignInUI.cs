using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class SignInUI : MonoBehaviour
{
    public Button CreateAccountBtn;
    public Button LogInBtn;
    public Button FindBtn;
    public Button DoubleCheckBtn;

    public InputField IDInput;
    public InputField PWInput;
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
    private bool _matchPassword;
    private void Start()
    {

        _connectionText = Resources.Load<TextAsset>("Connection");
        _insertAccountText = Resources.Load<TextAsset>("InsertAccount");
        _insertScoreText = Resources.Load<TextAsset>("InsertRanking");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _selectString = _selectText.text;

        int _signInChildIndex = IDInput.transform.childCount - 1;

        _idErrorText = IDInput.transform.GetChild(_signInChildIndex).gameObject;
        _pwErrorText = PWInput.transform.GetChild(_signInChildIndex).gameObject;
        _emailErrorText = EmailInput.transform.GetChild(_signInChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText.SetActive(false);
        _emailErrorText.SetActive(false);
    }
    private void OnEnable()
    {

        CreateAccountBtn.onClick.AddListener(CreateAccount);
        LogInBtn.onClick.AddListener(LoadLogIn);
        FindBtn.onClick.AddListener(LoadFind);
        DoubleCheckBtn.onClick.AddListener(DoubleCheck);

        PWInput.onValueChange.AddListener(CheckPassword);
        PWCheckInput.onValueChange.AddListener(CheckPassword);

        _idErrorText?.SetActive(false);
        _pwErrorText?.SetActive(false);
        _emailErrorText?.SetActive(false);

        _hasDoubleCheck = false;
        _matchPassword = false;
    }

    public void CreateAccount()
    {
        if(_hasDoubleCheck && _matchPassword)
        {
            _insertAccountString = _insertAccountText.text + $"('{IDInput.text}', '{PWInput.text}', '{EmailInput.text}');";
            _insertScoreString = _insertScoreText.text + $"('{IDInput.text}');";
            Debug.Log(_insertAccountString);
            Debug.Log(_insertScoreString);
            using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                MySqlCommand _insertAccountCommand = new MySqlCommand(_insertAccountString, _sqlConnection);
                _insertAccountCommand.ExecuteNonQuery();
            }
            using (MySqlConnection _sqlconnection = new MySqlConnection(_connectionString))
            {
                _sqlconnection.Open();
                MySqlCommand _insertScoreCommand = new MySqlCommand(_insertScoreString, _sqlconnection);
                _insertScoreCommand.ExecuteNonQuery();
            }
            _hasDoubleCheck = false;
        }
        else
        {
            Debug.Log("더블첵해야함");
        }
    }
    
    public void LoadLogIn() => UIManager.Instance.LoadUI(EUIIndex.LogIn);
    public void LoadFind() => UIManager.Instance.LoadUI(EUIIndex.Find);

    public DataSet GetUserData()
    {

        DataSet _dataSet = new DataSet();

        using (MySqlConnection _connection = new MySqlConnection(_connectionString))
        {
            _connection.Open();
            MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(_selectString, _connection);

            _dataAdapter.Fill(_dataSet);
        }
        return _dataSet;
    }
    public void DoubleCheck()
    {
        
         DataSet _dataSet;
         _dataSet = GetUserData();
         Debug.Log(_dataSet);
         bool _check = false;

         foreach (DataRow _dataRow in _dataSet.Tables[0].Rows)
         {
             if (_dataRow["ID"].ToString() == IDInput.text)
             {
                _check = true;
                break;
             }
         }
         
        if(!_check)
        {
            Debug.Log("사용 가능");
            _hasDoubleCheck = true;
            _idErrorText.SetActive(false);
        }
        else
        {
            Debug.Log("사용 불가능");
            _hasDoubleCheck = false;
            _idErrorText.SetActive(true);
        }
        
    }
    public void CheckPassword(string pw)
    {
        if (PWInput.text == PWCheckInput.text)
        {
            _matchPassword = true;
            _pwErrorText.SetActive(false);
        }
        else
        {
            _matchPassword = false;
            _pwErrorText.SetActive(true);
        }
    }

    private void OnDisable()
    {
        IDInput.text = "";
        PWInput.text = "";
        PWCheckInput.text = "";
        EmailInput.text = "";

        CreateAccountBtn.onClick.RemoveListener(CreateAccount);
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        FindBtn.onClick.RemoveListener(LoadFind);
        DoubleCheckBtn.onClick.RemoveListener(DoubleCheck);

        PWInput.onValueChange.RemoveListener(CheckPassword);
        PWCheckInput.onValueChange.RemoveListener(CheckPassword);
    }
}
