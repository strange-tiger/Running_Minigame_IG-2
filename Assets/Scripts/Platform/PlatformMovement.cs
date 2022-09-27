using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Platform Spec")]
    [SerializeField] private static float s_moveSpeed = 5f;
    public static float MoveSpeed
    {
        get => s_moveSpeed;
        set { s_moveSpeed = value; }
    }

    [SerializeField] private static float s_deactiveZPosition = -16f;
    public static float DeactiveZPosition
    {
        get => s_deactiveZPosition;
        private set { s_deactiveZPosition = value; }
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

        for (int i = 0; i < coinScripts.Length; ++i)
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
        foreach (var coin in _coins)
        {
            coin.SetActive(true);
        }
        MoveSpeed = 5f;
    }

    private void Update()
    {
        float deltaZPosition = - MoveSpeed * Time.deltaTime;
        transform.Translate(0f, 0f, deltaZPosition);

        if (transform.position.z < DeactiveZPosition)
        {
            gameObject.SetActive(false);
            _platformManager.ReturnPlatformToPool(gameObject);
        }
    }
}
