using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private GameObject[] _coins;
    private PlatformManager _platformManager;
    
    private void Awake()
    {
        GetMyCoins();
        _platformManager = GetComponentInParent<PlatformManager>();
    }

    private void GetMyCoins()
    {
        Coin[] coinScripts = GetComponentsInChildren<Coin>();
        _coins = new GameObject[coinScripts.Length];   

        for(int i = 0; i < coinScripts.Length; ++i)
        {
            _coins[i] = coinScripts[i].gameObject;
        }
    }

    private void OnEnable()
    {
        ResetPlatform();
    }

    private void ResetPlatform()
    {
        foreach(var coin in _coins)
        {
            coin.SetActive(true);
        }
    }

    private void Update()
    {
        float deltaZPosition = -PlatformManager.PlatformMoveSpeed * Time.deltaTime;
        transform.Translate(0f, 0f, deltaZPosition);

        if(transform.position.z < PlatformManager.PlatformDisableZPosition)
        {
            gameObject.SetActive(false);
            if (_platformManager)
            {
                _platformManager.ReturnPlatformToPool(gameObject);
            }
        }
    }
}
