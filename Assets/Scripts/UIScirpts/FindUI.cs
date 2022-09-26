using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;

public class FindUI : MonoBehaviour
{
    public LogInUIManager LogInUIManager;

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
        
        int findIdChildIndex = Pw_IDInput.transform.childCount - 1;

        _id_EmailErrorText = Id_EmailInput.transform.GetChild(findIdChildIndex).gameObject;
        _pw_EmailErrorText = Pw_EmailInput.transform.GetChild(findIdChildIndex).gameObject;
        _pw_IDErrorText = Pw_IDInput.transform.GetChild(findIdChildIndex).gameObject;
        _id_EmailErrorText.SetActive(false);
        _pw_EmailErrorText.SetActive(false);
        _pw_IDErrorText.SetActive(false);
    }
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        Id_EnterBtn.onClick.AddListener(FindID);
        Pw_EnterBtn.onClick.AddListener(FindPW);

        _id_EmailErrorText?.SetActive(false);
        _pw_EmailErrorText?.SetActive(false);
        _pw_IDErrorText?.SetActive(false);
    }

    public void LoadLogIn() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LogIn);
    public void LoadSignIn() => LogInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SignIn);
    private DataSet GetUserData()
    {
        DataSet dataSet = new DataSet();
        using(MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
        {
            sqlConnection.Open();

            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(_selectString, sqlConnection);
            sqlDataAdapter.Fill(dataSet);
        }
        return dataSet;

    }
    public void FindID()
    {
        DataSet findIdDataSet;

        findIdDataSet = GetUserData();

        foreach(DataRow dataRow in findIdDataSet.Tables[0].Rows)
        {
            if(dataRow["Email"].ToString() == Id_EmailInput.text)
            {
                Id_Output.text = dataRow["ID"].ToString();
                _id_EmailErrorText.SetActive(false);

                return;
            }
        }
        _id_EmailErrorText.SetActive(true);
    }

    public void FindPW()
    {
        DataSet findPwDataSet = GetUserData();

        bool emailExist = false;
        bool idExist = false;
        bool curEmailExist = false;
        bool curIdExist = false;
        foreach (DataRow dataRow in findPwDataSet.Tables[0].Rows)
        {
            if (dataRow["Email"].ToString() == Pw_EmailInput.text)
            {
                emailExist = true;
                curEmailExist = true;
            }
            if (dataRow["ID"].ToString() == Pw_IDInput.text)
            {
                idExist = true;
                curIdExist = true;
            }

            if(curEmailExist && curIdExist)
            {
                Pw_Output.text = dataRow["Password"].ToString();
                break;
            }
            curEmailExist = false;
            curIdExist = false;
        }

        _pw_EmailErrorText.SetActive(!emailExist);
        _pw_IDErrorText.SetActive(!idExist);
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
