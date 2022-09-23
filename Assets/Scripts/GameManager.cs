using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
    private PlayerHealth _playerHealth;

    public UnityEvent OnGetCoin = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();

    public int _highScore = 0;
    public int HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            PlayerPrefs.SetInt("HighScore", value);
        }
    }
    
    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnGetCoin.Invoke();
        }
    }

    private void OnEnable()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }
    void Update()
    {
        
    }

    public void GetCoin()
    {
        ++Score;
    }

    public void OnCrashObstacle()
    {
        _playerHealth.Die();
        PlatformManager.PlatformMoveSpeed = 0f;
    }

    public void GameOver()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        HighScore = PlayerPrefs.GetInt("HighScore");

        OnGameOver.Invoke();
    }
}
