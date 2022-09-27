using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MySql;

public class GameManager : SingletonBehaviour<GameManager>
{
    public PlayerHealth PlayerHealth { get; set; }
    public GetRanking GetRanking { get; private set; }

    public void Awake()
    {
        MySqlSetting.Init();
        //MySqlSetting.AddNewAccount("abc", "123123", "bbb@gmail.com");

        //Debug.Log(MySqlSetting.ShowAccountList().ToString());
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
