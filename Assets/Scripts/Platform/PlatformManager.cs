using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private enum EPositionIndex
    {
        Rear,
        Mid,
        Front,
        Start,
        Max,
    }

    [Header("Platform Position")]
    [SerializeField] private Vector3 _platformPoolPosition;
    [SerializeField]
    private Vector3[] _platformStartPositions = {
        new Vector3(0f, 0f, 80f),
        new Vector3(0f, 0f, 56f),
        new Vector3(0f, 0f, 32f),
        new Vector3(0f, 0f, 8f)
    };

    // Platforms
    private List<GameObject> _platforms;

    private void Awake()
    {
        SetPlatformList();

        // ½ÃÀÛ Platform ¼³Á¤
        _platforms[_platforms.Count - 1].SetActive(true);
        _platforms[_platforms.Count - 1].transform.position = _platformStartPositions[(int)EPositionIndex.Start];

        // ³ª¸ÓÁö ·£´ý ¼³Á¤
        for (int i = 0; i < _platformStartPositions.Length - 1; ++i)
        {
            SelectNextPlatform(_platformStartPositions[i]);
        }
    }
    private void SetPlatformList()
    {
        _platforms = new List<GameObject>();

        PlatformMovement[] platformScripts = GetComponentsInChildren<PlatformMovement>();

        foreach (PlatformMovement script in platformScripts)
        {
            GameObject platform = script.gameObject;

            platform.SetActive(false);
            platform.transform.position = _platformPoolPosition;

            _platforms.Add(platform);
        }
    }

    private GameObject SelectNextPlatform(Vector3 startPosition)
    {
        // ÇÃ·§Æû ¼±ÅÃ
        int nextPlatformNumber;
        do {
            nextPlatformNumber = Random.Range(0, _platforms.Count - 1);
        } while (_platforms[nextPlatformNumber].activeSelf);

        // ¼±ÅÃµÈ ÇÃ·§Æû ¼³Á¤
        GameObject nextPlatform = _platforms[nextPlatformNumber];
        nextPlatform.transform.position = startPosition;
        nextPlatform.SetActive(true);

        return nextPlatform;
    }

    public void ReturnPlatformToPool(GameObject platform)
    {
        platform.transform.position = _platformPoolPosition;

        SelectNextPlatform(_platformStartPositions[(int)EPositionIndex.Rear]);
    }
}
