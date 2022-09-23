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
    private void Start()
    {

        _connectionText = Resources.Load<TextAsset>("Connection");
        _insertAccountText = Resources.Load<TextAsset>("InsertAccount");
        _insertScoreText = Resources.Load<TextAsset>("InsertRanking");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _selectString = _selectText.text;

    }
    private void OnEnable()
    {
        int _signInChildIndex = IDInput.transform.childCount - 1;

        CreateAccountBtn.onClick.AddListener(CreateAccount);
        LogInBtn.onClick.AddListener(LoadLogIn);
        FindBtn.onClick.AddListener(LoadFind);
        DoubleCheckBtn.onClick.AddListener(DoubleCheck);

        _idErrorText = IDInput.transform.GetChild(_signInChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText = PWInput.transform.GetChild(_signInChildIndex).gameObject;
        _pwErrorText.SetActive(false);
        _emailErrorText = EmailInput.transform.GetChild(_signInChildIndex).gameObject;
        _emailErrorText.SetActive(false);
    }

    public void CreateAccount()
    {
        if(_hasDoubleCheck)
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
        int _checkCount = 0;

         foreach (DataRow _dataRow in _dataSet.Tables[0].Rows)
         {
             if (_dataRow["ID"].ToString() != IDInput.text)
             {
             }
             else
             {
                _checkCount++;
             }

         }
         
        if(_checkCount == 0)
        {
           Debug.Log("사용 가능");
          _hasDoubleCheck = true;
        }
        else
        {
            Debug.Log("사용 불가능");
            _hasDoubleCheck = false;
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
    }
}
