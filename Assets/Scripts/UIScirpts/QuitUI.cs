using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitUI : MonoBehaviour
{
    public Button YesButton;
    public Button NoButton;

    private void OnEnable()
    {
        YesButton.onClick.AddListener(Quit);
        NoButton.onClick.AddListener(LoadLogIn);
    }

    public void Quit()
    {
        Debug.Log("Quit");

        Application.Quit();
    }
    public void LoadLogIn() => gameObject.SetActive(false);

    private void OnDisable()
    {
        YesButton.onClick.RemoveListener(Quit);
        NoButton.onClick.RemoveListener(LoadLogIn);
    }
}
