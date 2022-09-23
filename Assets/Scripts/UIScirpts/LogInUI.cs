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
    private bool _isExistId;
    private bool _canLogIn;

    private void Start()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");

    }
    private void OnEnable()
    {
        int _loginChildIndex = IDInput.transform.childCount - 1;

        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        FindBtn.onClick.AddListener(LoadFind);
        QuitBtn.onClick.AddListener(LoadQuit);

        _idErrorText = IDInput.transform.GetChild(_loginChildIndex).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText = PWInput.transform.GetChild(_loginChildIndex).gameObject;
        _pwErrorText.SetActive(false);

        _isExistId = false;

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

                Debug.Log("아이디 존재하지 않음");
        }
        else
        {
                Debug.Log("아이디 존재");

        }

        foreach(DataRow _dataRow in _logInDataSet.Tables[0].Rows)
        {
            if(_dataRow["Password"].ToString() == PWInput.text)
            {
                Debug.Log("로그인 완료");
            }
            else
            {
                Debug.Log("비번안맞음.");
            }
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
