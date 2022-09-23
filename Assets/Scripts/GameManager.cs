using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
    private PlayerHealth _playerHealth;
    public UnityEvent GetCoin = new UnityEvent();
    
    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            GetCoin.Invoke();
        }
    }

    private void OnEnable()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }
    void Update()
    {
        
    }

    public void OnGetCoin()
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

    }
}
