using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
    public PlayerHealth PlayerHealth { get; set; }
    
    private void OnEnable()
    {
        PlayerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void GetCoin()
    {
        PlayerHealth.GetCoin();
    }

    public void OnCrashObstacle()
    {
        PlatformManager.PlatformMoveSpeed = 0f;
        PlayerHealth.Die();
    }
}
