using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 60f;

    private void Update()
    {
        transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.GetCoin();

        gameObject.SetActive(false);
    }
}
