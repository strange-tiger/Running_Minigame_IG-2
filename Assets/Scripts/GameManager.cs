//#define _DEBUG_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
    public PlayerHealth PlayerHealth { get; set; }
    public GetRanking GetRanking { get; private set; }

    private void OnEnable()
    {
        PlayerHealth = FindObjectOfType<PlayerHealth>();
# if _DEBUG_MODE_
        PlayerPrefs.SetString("ID", "user123");
# endif

        GetRanking = new GetRanking();
        GetRanking.Init();
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
