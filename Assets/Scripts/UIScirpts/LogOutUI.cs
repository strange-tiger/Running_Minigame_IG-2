using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogOutUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private void OnEnable()
    {
        _yesButton.onClick.AddListener(LogOut);
        _noButton.onClick.AddListener(Close);
    }

    // PlayerPrefs에 저장되어 있는 ID 키를 제거해 로그아웃하고 Login 씬을 로드한다.
    public void LogOut()
    {
        // DB 연결 해제 (로그아웃)
        PlayerPrefs.DeleteKey("ID");
        
        SceneManager.LoadScene(0);
    }
    public void Close() => gameObject.SetActive(false);

    private void OnDisable()
    {
        _yesButton.onClick.RemoveListener(LogOut);
        _noButton.onClick.RemoveListener(Close);
    }
}
