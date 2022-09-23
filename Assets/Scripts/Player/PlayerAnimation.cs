using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public void GameOver()
    {
        GameManager.Instance.GameOver();
    }
}
