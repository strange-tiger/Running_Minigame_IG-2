using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;

public class RankUI : MonoBehaviour
{
    [SerializeField] private int _rankNumber = 5;

    private TextAsset _connectionText;
    private TextAsset _selectText;

    private Text[] _nicknameText;
    private Text[] _scoreText;

    private string _connectionString;
    private string _selectString;
    private void Awake()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("SelectRank");

        _nicknameText = new Text[_rankNumber];
        _scoreText = new Text[_rankNumber];

        for (int i = 0; i < _rankNumber; ++i)
        {
            _nicknameText[i] = transform.GetChild(i + 1).GetChild(1).GetComponent<Text>();
            _scoreText[i] = transform.GetChild(i + 1).GetChild(2).GetComponent<Text>();
        }
    }

    private void Update()
    {
        UpdateRanking();
    }

    private void UpdateRanking()
    {
        _selectString = _selectText.text + $" Order By High_Record DESC Limit 5;";

        using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            _sqlConnection.Open();
            MySqlCommand _readCommand = new MySqlCommand(_selectString, _sqlConnection);
            MySqlDataReader _dataReader = _readCommand.ExecuteReader();

            for (int i = 0; i < _rankNumber; ++i)
            {
                if (false == _dataReader.Read())
                {
                    break;
                }
                _nicknameText[i].text = _dataReader["ID"].ToString();
                _scoreText[i].text = _dataReader["High_Record"].ToString();
            }
            _sqlConnection.Close();
        }
    }
}
