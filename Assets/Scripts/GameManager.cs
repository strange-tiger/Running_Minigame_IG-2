using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    private PlayerHealth _playerHealth;

    private void OnEnable()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }
    void Update()
    {
        
    }

    public void OnGetCoin()
    {

    }

    public void OnCrashObstacle()
    {
        _playerHealth.Die();
    }
}
