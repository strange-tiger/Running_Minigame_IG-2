#define _DEBUG_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;
#if _DEBUG_MODE_
using MySql;
#endif

public class FindUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button _logInButton;
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _idEnterButton;
    [SerializeField] private Button _passwordEnterButton;

    [Header("Input Field")]
    [SerializeField] private InputField _idEmailInput;
    [SerializeField] private InputField _idOutput;
    [SerializeField] private InputField _passwordEmailInput;
    [SerializeField] private InputField _passwordIdInput;
    [SerializeField] private InputField _passwordOutput;

    private LogInUIManager _logInUIManager;

    private GameObject _idEmailErrorText;
    private GameObject _pwEmailErrorText;
    private GameObject _pwIdErrorText;

    private TextAsset _connectionText;
    private TextAsset _selectText;
    private string _connectionString;
    private string _selectString;

    private void Start()
    {
        _logInUIManager = GetComponentInParent<LogInUIManager>();

        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("Select");
        _selectString = _selectText.text + ";";

        int findIdChildIndex = _passwordIdInput.transform.childCount - 1;

        _idEmailErrorText = _idEmailInput.transform.GetChild(findIdChildIndex).gameObject;
        _pwEmailErrorText = _passwordEmailInput.transform.GetChild(findIdChildIndex).gameObject;
        _pwIdErrorText = _passwordIdInput.transform.GetChild(findIdChildIndex).gameObject;
        _idEmailErrorText.SetActive(false);
        _pwEmailErrorText.SetActive(false);
        _pwIdErrorText.SetActive(false);
    }
    private void OnEnable()
    {
        _logInButton.onClick.AddListener(LoadLogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _idEnterButton.onClick.AddListener(FindID);
        _passwordEnterButton.onClick.AddListener(FindPW);

        _idEmailErrorText?.SetActive(false);
        _pwEmailErrorText?.SetActive(false);
        _pwIdErrorText?.SetActive(false);
    }

    public void LoadLogIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LogIn);
    public void LoadSignIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SignIn);
    private DataSet GetUserData()
    {
        DataSet dataSet = new DataSet();
        using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
        {
            sqlConnection.Open();

            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(_selectString, sqlConnection);
            sqlDataAdapter.Fill(dataSet);
        }
        return dataSet;

    }
    public void FindID()
    {
#if _DEBUG_MODE_
        if(!MySqlSetting.IsThereValue(MySqlSetting.EAccountColumnType.Email, _idEmailInput.text))
        {
            _idEmailErrorText.SetActive(true);
            return;
        }

        string id = MySqlSetting.GetValueByBase(
            MySqlSetting.EAccountColumnType.Email, _idEmailInput.text,
            MySqlSetting.EAccountColumnType.ID);

        _idOutput.text = id;
        _idEmailErrorText.SetActive(false);
#else
        DataSet findIdDataSet;

        findIdDataSet = GetUserData();

        foreach (DataRow dataRow in findIdDataSet.Tables[0].Rows)
        {
            if (dataRow["Email"].ToString() == _idEmailInput.text)
            {
                _idOutput.text = dataRow["ID"].ToString();
                _idEmailErrorText.SetActive(false);

                return;
            }
        }
        _idEmailErrorText.SetActive(true);
#endif
    }

    public void FindPW()
    {
#if _DEBUG_MODE_
        if(!MySqlSetting.IsThereValue(MySqlSetting.EAccountColumnType.Email, _passwordEmailInput.text))
        {
            _pwEmailErrorText.SetActive(true);
            _pwIdErrorText.SetActive(false);
            return;
        }

        if(!MySqlSetting.IsThereValue(MySqlSetting.EAccountColumnType.ID, _passwordIdInput.text))
        {
            _pwEmailErrorText.SetActive(false);
            _pwIdErrorText.SetActive(true);
            return;
        }

        if(!MySqlSetting.CheckValueByBase(MySqlSetting.EAccountColumnType.Email, _passwordEmailInput.text,
            MySqlSetting.EAccountColumnType.ID, _passwordIdInput.text))
        {
            _pwEmailErrorText.SetActive(true);
            _pwIdErrorText.SetActive(true);
            return;
        }

        _pwEmailErrorText.SetActive(false);
        _pwIdErrorText.SetActive(false);

        string password = MySqlSetting.GetValueByBase(MySqlSetting.EAccountColumnType.ID, _passwordIdInput.text, MySqlSetting.EAccountColumnType.Password);
        _passwordOutput.text = password;
#else

        DataSet findPwDataSet = GetUserData();

        bool emailExist = false;
        bool idExist = false;
        bool curEmailExist = false;
        bool curIdExist = false;
        foreach (DataRow dataRow in findPwDataSet.Tables[0].Rows)
        {
            if (dataRow["Email"].ToString() == _passwordEmailInput.text)
            {
                emailExist = true;
                curEmailExist = true;
            }
            if (dataRow["ID"].ToString() == _passwordIdInput.text)
            {
                idExist = true;
                curIdExist = true;
            }

            if (curEmailExist && curIdExist)
            {
                _passwordOutput.text = dataRow["Password"].ToString();
                break;
            }
            curEmailExist = false;
            curIdExist = false;
        }

        _pwEmailErrorText.SetActive(!emailExist);
        _pwIdErrorText.SetActive(!idExist);
#endif
    }

    private void OnDisable()
    {
        _idEmailInput.text = "";
        _idOutput.text = "";
        _passwordEmailInput.text = "";
        _passwordIdInput.text = "";
        _passwordOutput.text = "";
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _idEnterButton.onClick.RemoveListener(FindID);
        _passwordEnterButton.onClick.RemoveListener(FindPW);
    }
}
