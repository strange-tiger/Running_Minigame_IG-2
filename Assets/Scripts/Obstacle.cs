using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("¿Ö ¾ÈµÊ");
        GameManager.Instance.OnCrashObstacle();
    }
}
