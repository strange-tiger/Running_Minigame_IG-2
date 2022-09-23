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
    [SerializeField] private GameObject newHighScoreInfoText;

    private void Awake()
    {
        inGamePanel.SetActive(true);
        MenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGetCoin.RemoveListener(ResetScore);
        GameManager.Instance.OnGetCoin.AddListener(ResetScore);

        GameManager.Instance.OnGameOver.RemoveListener(ShowGameOverPanel);
        GameManager.Instance.OnGameOver.AddListener(ShowGameOverPanel);
    }

    private void ResetScore()
    {
        inGameScoreText.text = GameManager.Instance.Score.ToString();
    }

    private void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        int score = GameManager.Instance.Score;
        int highScore = GameManager.Instance.HighScore;
        gameOverScoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();

        if(score > highScore)
        {
            newHighScoreInfoText.SetActive(true);
            GameManager.Instance.HighScore = score;
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGetCoin.RemoveListener(ResetScore);
        GameManager.Instance.OnGameOver.RemoveListener(ShowGameOverPanel);
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
