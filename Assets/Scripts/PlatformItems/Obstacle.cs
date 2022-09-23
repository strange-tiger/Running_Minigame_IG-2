using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.OnCrashObstacle();

        other.gameObject.transform.parent = gameObject.transform.parent;
    }
}
