using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("Platform Spec")]
    [SerializeField] private static float _platformMoveSpeed = 5f;
    public static float PlatformMoveSpeed 
    { 
        get => _platformMoveSpeed; 
        private set { _platformMoveSpeed = value; } 
    }


    [Header("Platform Position")]
    [SerializeField] private static float _platformDisableZPosition = -16f;
    public static float PlatformDisableZPosition 
    { 
        get => _platformDisableZPosition; 
        private set { _platformDisableZPosition = value; } 
    }

    [SerializeField] private Vector3 _platformPoolPosition;
    [SerializeField] private Vector3[] _platformStartPositions = {
        new Vector3(0f, 0f, 80f),
        new Vector3(0f, 0f, 56f),
        new Vector3(0f, 0f, 32f),
        new Vector3(0f, 0f, 8f)
    };

    private List<GameObject> _platforms;
    private int _currentShownPlatformCount = 0;

    private void Awake()
    {
        SetPlatformList();

        _platforms[_platforms.Count - 1].SetActive(true);
        _platforms[_platforms.Count - 1].transform.position = _platformStartPositions[_platformStartPositions.Length - 1];

        for(int i = 0; i<_platformStartPositions.Length - 1; ++i)
        {
            SelectNextPlatform(_platformStartPositions[i]);
        }
    }
    private void SetPlatformList()
    {
        _platforms = new List<GameObject>();

        PlatformMovement[] platformScripts = GetComponentsInChildren<PlatformMovement>();

        for(int i = 0; i < platformScripts.Length; ++i)
        {
            GameObject platform = platformScripts[i].gameObject;

            platform.SetActive(false);
            platform.transform.position = _platformPoolPosition;
            
            _platforms.Add(platform);
        }
    }

    private GameObject SelectNextPlatform(Vector3 startPosition)
    {
        int nextPlatformNumber;
        do
        {
            nextPlatformNumber = Random.Range(0, _platforms.Count - 1);
        } while (_platforms[nextPlatformNumber].activeSelf);

        GameObject nextPlatform = _platforms[nextPlatformNumber];
        nextPlatform.transform.position = startPosition;
        nextPlatform.SetActive(true);

        ++_currentShownPlatformCount;
        
        return nextPlatform;
    }

    public void ReturnPlatformToPool(GameObject platform)
    {
        platform.transform.position = _platformPoolPosition;
        --_currentShownPlatformCount;

        CheckPlatformCount();
    }

    private void CheckPlatformCount()
    {
        if(_currentShownPlatformCount > 4)
        {
            return;
        }

        SelectNextPlatform(_platformStartPositions[0]);
    }
}
