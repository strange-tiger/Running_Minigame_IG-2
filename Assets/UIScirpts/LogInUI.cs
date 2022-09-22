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

    private bool _rightPW;
    private void OnEnable()
    {
        LogInBtn.onClick.AddListener(LoadLogIn);
        SignInBtn.onClick.AddListener(LoadSignIn);
        FindBtn.onClick.AddListener(LoadFind);
        QuitBtn.onClick.AddListener(LoadQuit);
    }

    public DataSet GetUser()
    {
        string connectString = string.Format("Server={0};Database={1};Uid ={2};Pwd={3};", "127.0.0.1",
"Running", "root", "19950417");
        string sql = "select * from Account";
        DataSet ds = new DataSet();

        using (MySqlConnection conn = new MySqlConnection(connectString))
        {
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            
            da.Fill(ds);
        }
        return ds;
    }
    public void LoadLogIn()
    {

        DataSet _dataSet;
        _dataSet = GetUser();
        Debug.Log(_dataSet);
      

        foreach(DataRow row in _dataSet.Tables[0].Rows)
        {
            if (row["Password"].ToString() == PWInput.text)
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
