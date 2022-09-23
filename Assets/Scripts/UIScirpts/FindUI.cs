using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;

public class FindUI : MonoBehaviour
{
    public Button LogInBtn;
    public Button SignInBtn;
    public Button Id_EnterBtn;
    public Button Pw_EnterBtn;

    public InputField Id_EmailInput;
    public InputField Id_Output;
    public InputField Pw_EmailInput;
    public InputField Pw_IDInput;
    public InputField Pw_Output;

    private GameObject _id_EmailErrorText;
    private GameObject _pw_EmailErrorText;
    private GameObject _pw_IDErrorText;

    private TextAsset _connectionText;
    private TextAsset _selectText;
    private string _connectionString;
    private string _selectString;

    private void Start()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _selectString = _selectText.text + ";";
    }
    private void OnEnable()
    {
        int _findIdChildIndex = Pw_IDInput.transform.childCount - 1;

        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        Id_EnterBtn.onClick.AddListener(FindID);
        Pw_EnterBtn.onClick.AddListener(FindPW);

        _id_EmailErrorText = Id_EmailInput.transform.GetChild(_findIdChildIndex).gameObject;
        _id_EmailErrorText.SetActive(false);
        _pw_EmailErrorText = Pw_EmailInput.transform.GetChild(_findIdChildIndex).gameObject;
        _pw_EmailErrorText.SetActive(false);
        _pw_IDErrorText = Pw_IDInput.transform.GetChild(_findIdChildIndex).gameObject;
        _pw_IDErrorText.SetActive(false);
    }

    public void LoadLogIn() => UIManager.Instance.LoadUI(EUIIndex.LogIn);
    public void LoadSignIn() => UIManager.Instance.LoadUI(EUIIndex.SignIn);
    private DataSet GetUserData()
    {
        DataSet _dataSet = new DataSet();
        using(MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            _sqlConnection.Open();

            MySqlDataAdapter _sqlDataAdapter = new MySqlDataAdapter(_selectString, _sqlConnection);
            _sqlDataAdapter.Fill(_dataSet);
        }
        return _dataSet;

    }
    public void FindID()
    {
        DataSet _findIdDataSet;

        _findIdDataSet = GetUserData();

        foreach(DataRow _dataRow in _findIdDataSet.Tables[0].Rows)
        {
            if(_dataRow["Email"].ToString() == Id_EmailInput.text)
            {
                Id_Output.text = _dataRow["ID"].ToString();
                break;
            }
        }
    }

    public void FindPW()
    {
        DataSet _findPwDataSet;

        _findPwDataSet = GetUserData();

        foreach (DataRow _dataRow in _findPwDataSet.Tables[0].Rows)
        {
            if (_dataRow["Email"].ToString() == Pw_EmailInput.text && _dataRow["ID"].ToString() == Pw_IDInput.text)
            {
                Pw_Output.text = _dataRow["Password"].ToString();
                break;
            }
        }
    }

    private void OnDisable()
    {
        Id_EmailInput.text = "";
        Id_Output.text = "";
        Pw_EmailInput.text = "";
        Pw_IDInput.text = "";
        Pw_Output.text = "";
        LogInBtn.onClick.RemoveListener(LoadLogIn);
        SignInBtn.onClick.RemoveListener(LoadSignIn);
        Id_EnterBtn.onClick.RemoveListener(FindID);
        Pw_EnterBtn.onClick.RemoveListener(FindPW);
    }
}
