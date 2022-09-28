using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;
using MySql.Data.MySqlClient;
using Asset.MySql;

public class RankUI : MonoBehaviour
{
    private TextAsset _connectionText;
    private TextAsset _selectText;

    private TextMeshProUGUI[] _nicknameText;
    private TextMeshProUGUI[] _scoreText;

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

        _nicknameText = new TextMeshProUGUI[_rankNumber];
        _scoreText = new TextMeshProUGUI[_rankNumber];

        for (int i = _rankNumber; i > 0; --i)
        {
            _nicknameText[i - 1] = transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            _scoreText[i - 1] = transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(UpdateRank());
    }

    public IEnumerator UpdateRank()
    {
        while (true)
        {
            UpdateRanking();
            yield return new WaitForSeconds(5);
        }
    }

    private void UpdateRanking()
    {
        List<Dictionary<string, string>> ranking = MySqlSetting.GetDataByOrderLimitRowCount
            (ERankingColumType.High_Record, _rankNumber,
            ERankingColumType.ID, ERankingColumType.High_Record);

        if(ranking.Count == 0)
        {
            Debug.LogError("랭킹 가져오는 것에 오류 있음");
            return;
        }

        for(int i = 0; i<_rankNumber; ++i)
        {
            _nicknameText[i].text = ranking[i]["ID"];
            _scoreText[i].text = ranking[i]["High_Record"];
        }
        //_selectString = _selectText.text + $" Order By High_Record DESC Limit 5;";
        //
        //using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        //{
        //    _sqlConnection.Open();
        //    MySqlCommand _readCommand = new MySqlCommand(_selectString, _sqlConnection);
        //    MySqlDataReader _dataReader = _readCommand.ExecuteReader();
        //
        //    for (int i = 0; i < _rankNumber; ++i)
        //    {
        //        if (false == _dataReader.Read())
        //        {
        //            break;
        //        }
        //        _nicknameText[i].text = _dataReader["ID"].ToString();
        //        _scoreText[i].text = _dataReader["High_Record"].ToString();
        //    }
        //    _sqlConnection.Close();
        //}
    }
}
