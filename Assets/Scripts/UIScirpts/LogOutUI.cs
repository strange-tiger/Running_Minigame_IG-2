using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogOutUI : MonoBehaviour
{
    public Button YesBtn;
    public Button NoBtn;

    private void OnEnable()
    {
        YesBtn.onClick.AddListener(LogOut);
        NoBtn.onClick.AddListener(Close);
    }

    public void LogOut()
    {
        // DB 연결 해제 (로그아웃)
        SceneManager.LoadScene(0);
    }
    public void Close() => gameObject.SetActive(false);

    private void OnDisable()
    {
        YesBtn.onClick.RemoveListener(LogOut);
        NoBtn.onClick.RemoveListener(Close);
    }
}
