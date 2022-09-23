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
    private string _sqlConnectionString;
    private string _sqlSelectString;
    private void Start()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _sqlConnectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _sqlSelectString = _selectText.text;

    }
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        FindBtn.onClick.AddListener(LoadFind);
        QuitBtn.onClick.AddListener(LoadQuit);

        _idErrorText = IDInput.transform.GetChild(2).gameObject;
        _idErrorText.SetActive(false);
        _pwErrorText = PWInput.transform.GetChild(2).gameObject;
        _pwErrorText.SetActive(false);
    }

    public DataSet GetUser()
    {
         
        DataSet _sqlDataSet = new DataSet();

        using (MySqlConnection _sqlConnection = new MySqlConnection(_sqlConnectionString))
        {
            _sqlConnection.Open();
            MySqlDataAdapter _sqlDataAdapter = new MySqlDataAdapter(_sqlSelectString, _sqlConnection);
            
            _sqlDataAdapter.Fill(_sqlDataSet);
        }
        return _sqlDataSet;
    }
    public void LoadLogIn()
    {

        DataSet _dataSet;
        _dataSet = GetUser();
        Debug.Log(_dataSet);
      

        foreach(DataRow _sqlDataRow in _dataSet.Tables[0].Rows)
        {
            if (_sqlDataRow["Password"].ToString() == PWInput.text)
            {
                Debug.Log("good");
            }

        }

    }
    
    public void LoadSignIn() => UIManager.Instance.LoadUI(EUIIndex.SignIn); 
    public void LoadFind() => UIManager.Instance.LoadUI(EUIIndex.Find); 
    public void LoadQuit() => UIManager.Instance.LoadQuitCheck();

    private void OnDisable()
    {
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        SignInBtn.onClick.RemoveListener(LoadSignIn);
        FindBtn.onClick.RemoveListener(LoadFind);
        QuitBtn.onClick.RemoveListener(LoadQuit);
    }
}
