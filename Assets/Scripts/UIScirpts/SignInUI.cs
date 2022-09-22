using System.Collections;
using System.Collections.Generic;
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
        string connectString = string.Format("Server={0};Database={1};Uid ={2};Pwd={3};", "127.0.0.1",
"Running", "root", "19950417");
        string _insertAccount = $"Insert Into Account (ID,Password,Emain) values ('{IDInput.text}','{PWInput.text}','{EmailInput.text}');";
        string _insertData = $"Insert Into Ranking (ID) values ('{IDInput.text}');";

        using (MySqlConnection _mySqlConnect = new MySqlConnection(connectString))
        {
            _mySqlConnect.Open();
            MySqlCommand _insertAccountCommand = new MySqlCommand(_insertAccount, _mySqlConnect);
            _insertAccountCommand.ExecuteNonQuery();
        }
        using (MySqlConnection _mySqlConnect = new MySqlConnection(connectString))
        {
            _mySqlConnect.Open();
            MySqlCommand _insertRecordCommand = new MySqlCommand(_insertData, _mySqlConnect);
            _insertRecordCommand.ExecuteNonQuery();
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
