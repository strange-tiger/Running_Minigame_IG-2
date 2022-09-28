using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using Asset.MySql;

public class GetRanking
{
    private string _userId;
    private bool _hasUserId = false;

    // 최고 기록
    private int _highScore;
    public int HighScore
    {
        get => GetHighScore();
        private set
        {
            _highScore = value;
        }
    }

    public void Init()
    {
        SetSqlAssets();
    }

    private string GetPlayerId()
    {
        if (_hasUserId)
        {
            return _userId;
        }

        Debug.Assert(PlayerPrefs.HasKey("ID"), "플레이어 ID가 없음");

        if (PlayerPrefs.HasKey("ID"))
        {
            _userId = PlayerPrefs.GetString("ID");
            _hasUserId = true;
        }

        return _userId;
    }

    private TextAsset _connectionText;
    private string _connectionString;

    private TextAsset _updateScoreText;

    private TextAsset _selectScoreText;
    private string _selectScoreString;

    private void SetSqlAssets()
    {
        _connectionText = Resources.Load<TextAsset>("Connection");
        _connectionString = _connectionText.text;

        _updateScoreText = Resources.Load<TextAsset>("UpdateRanking");

        _selectScoreText = Resources.Load<TextAsset>("SelectMyRanking");
        _selectScoreString = _selectScoreText.text + $"'{GetPlayerId()}'";
    }

    public void SetNewHighScore(int newHighScore)
    {
        Debug.Assert(newHighScore > HighScore,
            $"새로운 점수 {newHighScore} 보다 기존 점수 {HighScore}가 더 높음");

        if(!MySqlSetting.UpdateValueByBase(ERankingColumType.ID, GetPlayerId(), 
            ERankingColumType.High_Record, newHighScore))
        {
            Debug.LogError("입력 오류");
            return;
        }
    }

    private int GetHighScore()
    {
        string highScoreString = MySqlSetting.GetValueByBase(ERankingColumType.ID, GetPlayerId(), ERankingColumType.High_Record);
        if (highScoreString == null)
        {
            Debug.LogError("최고 기록을 가져올 수 없음");
            return -1;
        }

        _highScore = int.Parse(highScoreString);

        return _highScore;
    }
}
