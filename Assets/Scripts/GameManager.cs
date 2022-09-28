using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Asset.MySql;

public class GameManager : SingletonBehaviour<GameManager>
{
    public PlayerHealth PlayerHealth { get; set; }
    public GetRanking GetRanking { get; private set; }

    private void OnEnable()
    {
        MySqlSetting.Init();
    }
    public void GetCoin()
    {
        PlayerHealth.GetCoin();
    }

    public void OnCrashObstacle()
    {
        PlatformMovement.MoveSpeed = 0f;
        PlayerHealth.Die();
    }

    public void LogInInit()
    {
        GetRanking = new GetRanking();
        GetRanking.Init();
    }
}
