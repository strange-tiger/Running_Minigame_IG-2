using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using MySql.Data.MySqlClient;

public class WaitingUI : MonoBehaviour
{
    public enum EWaitingUIIndex
    {
        Quit,
        LogOut,
        Max
    }

    public GameObject LogOutUI;
    public GameObject QuitUI;

    public Button StartBtn;
    public Button QuitBtn;
    public Button LogOutBtn;

    private TextAsset _connectionText;
    private TextAsset _selectText;

    private Text[] _nicknameText = new Text[5];
    private Text[] _scoreText = new Text[5];

    private string _connectionString;
    private string _selectString;
    private void Awake()
    {
        StartBtn.onClick.AddListener(StartGame);
        QuitBtn.onClick.AddListener(LoadQuit);
        LogOutBtn.onClick.AddListener(LoadLogOut);

        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("SelectRank");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadUI(EWaitingUIIndex ui)
    {
        LogOutUI.SetActive(false);
        QuitUI.SetActive(false);

        switch (ui)
        {
            case EWaitingUIIndex.Quit:
                QuitUI.SetActive(true);
                Debug.Log("Quit");
                break;
            case EWaitingUIIndex.LogOut:
                LogOutUI.SetActive(true);
                break;
            default:
                Debug.Assert(ui >= EWaitingUIIndex.Max, "Error: No UI Exists");
                break;
        }
    }

    public void LoadQuit() => LoadUI(EWaitingUIIndex.Quit);
    public void LoadLogOut() => LoadUI(EWaitingUIIndex.LogOut);

    private void UpdateRanking()
    {
        _selectString = _selectText.text + $" Order By High_Record ASC Limit 5;";

        using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            _sqlConnection.Open();
            MySqlCommand _readCommand = new MySqlCommand(_selectString, _sqlConnection);
            MySqlDataReader _dataReader = _readCommand.ExecuteReader();
            if (_dataReader.Read())
            {

            }
            _sqlConnection.Close();
        }
    }

    private void OnDisable()
    {
        StartBtn.onClick.RemoveListener(StartGame);
        QuitBtn.onClick.RemoveListener(LoadQuit);
        LogOutBtn.onClick.RemoveListener(LoadLogOut);
    }
}
