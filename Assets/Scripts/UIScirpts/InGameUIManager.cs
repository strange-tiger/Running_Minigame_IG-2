using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    [Header("InGamePanel")]
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("MenuPanel")]
    [SerializeField] private GameObject MenuPanel;

    [Header("GameOverPanel")]
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField]
    private void OnEnable()
    {
        GameManager.Instance.GetCoin.RemoveListener(ResetScore);
        GameManager.Instance.GetCoin.AddListener(ResetScore);
    }

    private void ResetScore()
    {
        scoreText.text = GameManager.Instance.Score.ToString();
    }

    private void OnDisable()
    {
        GameManager.Instance.GetCoin.RemoveListener(ResetScore);
    }
}
