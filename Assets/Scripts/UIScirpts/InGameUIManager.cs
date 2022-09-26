using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("InGamePanel")]
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI inGameScoreText;

    [Header("MenuPanel")]
    [SerializeField] private GameObject MenuPanel;

    [Header("GameOverPanel")]
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject newHighScoreText;

    private void Awake()
    {
        inGamePanel.SetActive(true);

        MenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        newHighScoreText.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Start()
    {
        GameManager.Instance.PlayerHealth.OnGetCoin.RemoveListener(ResetScore);
        GameManager.Instance.PlayerHealth.OnGetCoin.AddListener(ResetScore);

        GameManager.Instance.PlayerHealth.OnGameOver.RemoveListener(ShowGameOverPanel);
        GameManager.Instance.PlayerHealth.OnGameOver.AddListener(ShowGameOverPanel);

        GameManager.Instance.LogInInit();
    }

    private void ResetScore(int newScore)
    {
        inGameScoreText.text = newScore.ToString();
    }

    private void ShowGameOverPanel(int score)
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        // 스코어 저장
        gameOverScoreText.text = score.ToString();

        int highScore = GameManager.Instance.GetRanking.HighScore;
        highScoreText.text = highScore.ToString();

        if(score > highScore)
        {
            newHighScoreText.SetActive(true);
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
        MenuPanel.SetActive(true);
    }

    public void OnClickResume()
    {
        Time.timeScale = 1f;
        MenuPanel.SetActive(false);
    }

    public void OnClickExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
