using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class GetRanking
{
    private string _userId;
    private bool hasUserId = false;

    // �ְ� ���
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
        if(hasUserId)
        {
            return _userId;
        }

        Debug.Assert(PlayerPrefs.HasKey("ID"), "�÷��̾� ID�� ����");

        if (PlayerPrefs.HasKey("ID"))
        {
            _userId = PlayerPrefs.GetString("ID");
            hasUserId = true;
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
        Debug.Assert(newHighScore < HighScore,
            $"���ο� ���� {newHighScore} ���� ���� ���� {HighScore}�� �� ����");

        string updateScoreString = _updateScoreText.text + $"{newHighScore} WHERE ID = '{GetPlayerId()}'";
        using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            MySqlCommand _updateScoreCommand = new MySqlCommand(updateScoreString, _sqlConnection);
            _sqlConnection.Open();
            _updateScoreCommand.ExecuteNonQuery();
            _sqlConnection.Close();
        }
    }

    private int GetHighScore()
    {
        using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
        {
            _sqlConnection.Open();

            MySqlCommand _selectMyHighScore = new MySqlCommand(_selectScoreString, _sqlConnection);
            MySqlDataReader _readHighScore = _selectMyHighScore.ExecuteReader();

            Debug.Assert(_readHighScore.Read() == false, "��� ����");

            if(_readHighScore.Read() != false)
            {
                HighScore = int.Parse(_readHighScore["High_Record"].ToString());
            }

            _sqlConnection.Close();
        }

        return _highScore;
    }
}
