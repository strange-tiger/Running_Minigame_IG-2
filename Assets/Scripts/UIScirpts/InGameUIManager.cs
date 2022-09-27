using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("InGamePanel")]
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private TextMeshProUGUI _inGameScoreText;

    [Header("MenuPanel")]
    [SerializeField] private GameObject _menuPanel;

    [Header("GameOverPanel")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _gameOverScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private GameObject _newHighScoreText;

    private void Awake()
    {
        _inGamePanel.SetActive(true);

        _menuPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        _newHighScoreText.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Start()
    {
        GameManager.Instance.PlayerHealth.OnGetCoin.RemoveListener(ResetScore);
        GameManager.Instance.PlayerHealth.OnGetCoin.AddListener(ResetScore);

        GameManager.Instance.PlayerHealth.OnGameOver.RemoveListener(ShowGameOverPanel);
        GameManager.Instance.PlayerHealth.OnGameOver.AddListener(ShowGameOverPanel);
    }

    private void ResetScore(int newScore)
    {
        _inGameScoreText.text = newScore.ToString();
    }

    private void ShowGameOverPanel(int score)
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        // 스코어 저장
        _gameOverScoreText.text = score.ToString();

        int highScore = GameManager.Instance.GetRanking.HighScore;
        _highScoreText.text = highScore.ToString();

        if (score > highScore)
        {
            _newHighScoreText.SetActive(true);
            GameManager.Instance.GetRanking.SetNewHighScore(score);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerHealth.OnGetCoin.RemoveListener(ResetScore);
        GameManager.Instance.PlayerHealth.OnGameOver.RemoveListener(ShowGameOverPanel);
    }

    public void OnClickMenu()
    {
        Time.timeScale = 0f;
        _menuPanel.SetActive(true);
    }

    public void OnClickResume()
    {
        Time.timeScale = 1f;
        _menuPanel.SetActive(false);
    }

    public void OnClickExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
