using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using MySql;

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
    private GameObject _passwordErrorText;
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
    private bool _isMatchingPassword;
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
        _passwordErrorText = _passwordInput.transform.GetChild(signInChildIndex).gameObject;
        _emailErrorText = _emailInput.transform.GetChild(signInChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _passwordErrorText.SetActive(false);
        _emailErrorText.SetActive(false);
    }

    private void OnEnable()
    {
        _createAccountButton.onClick.AddListener(CreateAccount);
        _logInButton.onClick.AddListener(LoadLogIn);
        _findButton.onClick.AddListener(LoadFind);
        _idDoubleCheckButton.onClick.AddListener(IdDoubleCheck);
        _emailDoubleCheckButton.onClick.AddListener(EmailDoubleCheck);

        _passwordInput.onValueChanged.AddListener(CheckPassword);
        _passwordCheckInput.onValueChanged.AddListener(CheckPassword);

        _idErrorText?.SetActive(false);
        _passwordErrorText?.SetActive(false);
        _emailErrorText?.SetActive(false);

        _hasIdDoubleCheck = false;
        _hasEmailDoubleCheck = false;
        _isMatchingPassword = false;
    }

    // 입력된 계정 정보를 바탕으로 중복체크가 완료되었다면 계정 DB에 저장한다.
    public void CreateAccount()
    {
        if (_hasIdDoubleCheck && _hasEmailDoubleCheck && _isMatchingPassword)
        {
            if(MySqlSetting.AddNewAccount(_idInput.text, _passwordInput.text, _emailInput.text))
            {
                _idInput.text = "";
                _passwordInput.text = "";
                _passwordCheckInput.text = "";
                _emailInput.text = "";
            } 
            else
            {
                Debug.LogError("계정 추가 오류");
            }
        }
        else
        {
            Debug.Log("더블첵해야함");
        }
    }

    public void LoadLogIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LogIn);
    public void LoadFind() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.Find);

    //public DataSet GetUserData()
    //{
    //    DataSet dataSet = new DataSet();

    //    using (MySqlConnection connection = new MySqlConnection(_connectionString))
    //    {
    //        connection.Open();
    //        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(_selectString, connection);

    //        dataAdapter.Fill(dataSet);
    //    }
    //    return dataSet;
    //}
    public void IdDoubleCheck()
    {
        _hasIdDoubleCheck = !MySqlSetting.IsThereValue(MySqlSetting.EAccountColumnType.ID, _idInput.text);
        _idErrorText.SetActive(!_hasIdDoubleCheck);
    }

    public void EmailDoubleCheck()
    {
        _hasEmailDoubleCheck = !MySqlSetting.IsThereValue(MySqlSetting.EAccountColumnType.Email, _emailInput.text);
        _emailErrorText.SetActive(!_hasEmailDoubleCheck);
    }

    public void CheckPassword(string pw)
    {
        if (_passwordInput.text == _passwordCheckInput.text)
        {
            _isMatchingPassword = true;
            _passwordErrorText.SetActive(false);
        }
        else
        {
            _isMatchingPassword = false;
            _passwordErrorText.SetActive(true);
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

        _passwordInput.onValueChanged.RemoveListener(CheckPassword);
        _passwordCheckInput.onValueChanged.RemoveListener(CheckPassword);
    }
}
