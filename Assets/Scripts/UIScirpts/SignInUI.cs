using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using LitJson;

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

    private string _sqlConnectionString;
    private string _sqlInsertAccountString;
    private string _sqlInsertScoreString;
    private void Start()
    {

        _connectionText = Resources.Load<TextAsset>("Connection");
        _insertAccountText = Resources.Load<TextAsset>("InsertAccount");
        _insertScoreText = Resources.Load<TextAsset>("InsertRanking");
        _sqlConnectionString = _connectionText.text;

    }
    private void OnEnable()
    {
        CreateAccountBtn.onClick.AddListener(CreateAccount);
        LogInBtn.onClick.AddListener(LoadLogIn);
        FindBtn.onClick.AddListener(LoadFind);
        DoubleCheckBtn.onClick.AddListener(DoubleCheck);

        _idErrorText = IDInput.transform.GetChild(2).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText = PWInput.transform.GetChild(2).gameObject;
        _pwErrorText.SetActive(false);
        _emailErrorText = EmailInput.transform.GetChild(2).gameObject;
        _emailErrorText.SetActive(false);
    }

    public void CreateAccount()
    {
        _sqlInsertAccountString = _insertAccountText.text + $"('{IDInput.text}', '{PWInput.text}', '{EmailInput.text}');";
        _sqlInsertScoreString = _insertScoreText.text + $"('{IDInput.text}');";
        Debug.Log(_sqlInsertAccountString);
        Debug.Log(_sqlInsertScoreString);
        using (MySqlConnection _sqlConnection = new MySqlConnection(_sqlConnectionString))
        {
            _sqlConnection.Open();
            MySqlCommand _sqlInsertAccountCommand = new MySqlCommand(_sqlInsertAccountString, _sqlConnection);
            _sqlInsertAccountCommand.ExecuteNonQuery();
        }
        using (MySqlConnection _mySqlConnect = new MySqlConnection(_sqlConnectionString))
        {
            _mySqlConnect.Open();
            MySqlCommand _sqlInsertScoreCommand = new MySqlCommand(_sqlInsertScoreString, _mySqlConnect);
            _sqlInsertScoreCommand.ExecuteNonQuery();
        }
    }
    
    public void LoadLogIn() => UIManager.Instance.LoadUI(EUIIndex.LogIn);
    public void LoadFind() => UIManager.Instance.LoadUI(EUIIndex.Find);
    
    public void DoubleCheck()
    {

    }

    private void OnDisable()
    {
        CreateAccountBtn.onClick.RemoveListener(CreateAccount);
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        FindBtn.onClick.RemoveListener(LoadFind);
        DoubleCheckBtn.onClick.RemoveListener(DoubleCheck);
    }
}
