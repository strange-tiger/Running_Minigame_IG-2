using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;

public class FindUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button _logInButton;
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _idEnterButton;
    [SerializeField] private Button _passwordEnterButton;

    [Header("Input Field")]
    [SerializeField] private InputField _idEmailInput;
    [SerializeField] private InputField _idOutput;
    [SerializeField] private InputField _passwordEmailInput;
    [SerializeField] private InputField _passwordIdInput;
    [SerializeField] private InputField _passwordOutput;

    private LogInUIManager _logInUIManager;

    private GameObject _idEmailErrorText;
    private GameObject _pwEmailErrorText;
    private GameObject _pwIdErrorText;

    private void Start()
    {
        _logInUIManager = GetComponentInParent<LogInUIManager>();

        int findIdChildIndex = _passwordIdInput.transform.childCount - 1;

        _idEmailErrorText = _idEmailInput.transform.GetChild(findIdChildIndex).gameObject;
        _pwEmailErrorText = _passwordEmailInput.transform.GetChild(findIdChildIndex).gameObject;
        _pwIdErrorText = _passwordIdInput.transform.GetChild(findIdChildIndex).gameObject;
        _idEmailErrorText.SetActive(false);
        _pwEmailErrorText.SetActive(false);
        _pwIdErrorText.SetActive(false);
    }
    private void OnEnable()
    {
        _logInButton.onClick.AddListener(LoadLogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _idEnterButton.onClick.AddListener(FindID);
        _passwordEnterButton.onClick.AddListener(FindPW);

        _idEmailErrorText?.SetActive(false);
        _pwEmailErrorText?.SetActive(false);
        _pwIdErrorText?.SetActive(false);
    }

    public void LoadLogIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LogIn);
    public void LoadSignIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.SignIn);
    
    public void FindID()
    {
        if(!MySqlSetting.HasValue(EAccountColumns.Email, _idEmailInput.text))
        {
            _idEmailErrorText.SetActive(true);
            return;
        }

        string id = MySqlSetting.GetValueByBase(
            EAccountColumns.Email, _idEmailInput.text,
            EAccountColumns.ID);

        _idOutput.text = id;
        _idEmailErrorText.SetActive(false);
    }

    public void FindPW()
    {
        if(!MySqlSetting.HasValue(EAccountColumns.Email, _passwordEmailInput.text))
        {
            _pwEmailErrorText.SetActive(true);
            _pwIdErrorText.SetActive(false);
            return;
        }

        if(!MySqlSetting.HasValue(EAccountColumns.ID, _passwordIdInput.text))
        {
            _pwEmailErrorText.SetActive(false);
            _pwIdErrorText.SetActive(true);
            return;
        }

        if(!MySqlSetting.CheckValueByBase(EAccountColumns.Email, _passwordEmailInput.text,
            EAccountColumns.ID, _passwordIdInput.text))
        {
            _pwEmailErrorText.SetActive(true);
            _pwIdErrorText.SetActive(true);
            return;
        }

        _pwEmailErrorText.SetActive(false);
        _pwIdErrorText.SetActive(false);

        string password = MySqlSetting.GetValueByBase(EAccountColumns.ID, _passwordIdInput.text, EAccountColumns.Password);
        _passwordOutput.text = password;
    }

    private void OnDisable()
    {
        _idEmailInput.text = "";
        _idOutput.text = "";

        _passwordEmailInput.text = "";
        _passwordIdInput.text = "";
        _passwordOutput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _idEnterButton.onClick.RemoveListener(FindID);
        _passwordEnterButton.onClick.RemoveListener(FindPW);
    }
}
