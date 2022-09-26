using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Platform Spec")]
    [SerializeField] private static float _platformMoveSpeed = 5f;
    public static float PlatformMoveSpeed
    {
        get => _platformMoveSpeed;
        set { _platformMoveSpeed = value; }
    }

    [SerializeField] private static float _platformDisableZPosition = -16f;
    public static float PlatformDisableZPosition
    {
        get => _platformDisableZPosition;
        private set { _platformDisableZPosition = value; }
    }


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
        float deltaZPosition = -PlatformMoveSpeed * Time.deltaTime;
        transform.Translate(0f, 0f, deltaZPosition);

        if(transform.position.z < PlatformDisableZPosition)
        {
            gameObject.SetActive(false);
            _platformManager.ReturnPlatformToPool(gameObject);
        }
    }
}
