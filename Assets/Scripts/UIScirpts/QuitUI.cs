using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private void OnEnable()
    {
        _yesButton.onClick.AddListener(Quit);
        _noButton.onClick.AddListener(LoadLogIn);
    }

    public void Quit()
    {
        Debug.Log("Quit");

        Application.Quit();
    }
    public void LoadLogIn() => gameObject.SetActive(false);

    private void OnDisable()
    {
        _yesButton.onClick.RemoveListener(Quit);
        _noButton.onClick.RemoveListener(LoadLogIn);
    }
}
