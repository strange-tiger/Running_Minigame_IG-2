using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using MySql.Data.MySqlClient;

public class RankUI : MonoBehaviour
{
    private TextAsset _connectionText;
    private TextAsset _selectText;

    private Text[] _nicknameText;
    private Text[] _scoreText;

    private string _connectionString;
    private string _selectString;
    private int _rankNumber;
    private void Awake()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;
        _selectText = Resources.Load<TextAsset>("SelectRank");

        // 랭킹 보드는 위의 타이틀을 제외하고는 등수를 표시하는 오브젝트만 자식으로 가져야 한다.
        _rankNumber = transform.childCount - 1;
        
        _nicknameText = new Text[_rankNumber];
        _scoreText = new Text[_rankNumber];

        for (int i = _rankNumber; i > 0; --i)
        {
            _nicknameText[i - 1] = transform.GetChild(i).GetChild(1).GetComponent<Text>();
            _scoreText[i - 1] = transform.GetChild(i).GetChild(2).GetComponent<Text>();
        }

        StartCoroutine(UpdateRank());
    }

    public IEnumerator UpdateRank()
    {
        while(true)
        {
            UpdateRanking();
            yield return new WaitForSeconds(5000);
        }
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
