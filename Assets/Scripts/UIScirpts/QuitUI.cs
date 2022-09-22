using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitUI : MonoBehaviour
{
    public Button YesBtn;
    public Button NoBtn;

    private void OnEnable()
    {
        YesBtn.onClick.AddListener(Quit);
        NoBtn.onClick.AddListener(LoadLogIn);
    }

    public void Quit()
    {
        Debug.Log("Quit");

        Application.Quit();
    }
    public void LoadLogIn() => UIManager.Instance.LoadUI(EUIIndex.LogIn);

    private void OnDisable()
    {
        YesBtn.onClick.RemoveListener(LoadLogIn);
        NoBtn.onClick.RemoveListener(LoadLogIn);
    }
}
