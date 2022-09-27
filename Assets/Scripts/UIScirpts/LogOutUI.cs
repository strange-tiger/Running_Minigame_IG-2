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

    // PlayerPrefs�� ����Ǿ� �ִ� ID Ű�� ������ �α׾ƿ��ϰ� Login ���� �ε��Ѵ�.
    public void LogOut()
    {
        // DB ���� ���� (�α׾ƿ�)
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
