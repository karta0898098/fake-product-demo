using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPage : MonoBehaviour
{
    public InputField Name;
    public InputField Account;
    public InputField Password;
    public Button BtnBackToLogin;
    public Button BtnReigster;
    public Button BtnErrorOK;

    public GameObject ErrorPanel;
    public Text ErrorText;

    public GameObject LoginPage;
    public GameObject ProdctPage;

    public bool IsBusy;

    public void OnEnable()
    {
        BtnBackToLogin.onClick.AddListener(OnClickBtnBackToLogin);
        BtnReigster.onClick.AddListener(OnClickBtnRegister);
        BtnErrorOK.onClick.AddListener(OnClickBtnErrorOK);
    }

    public void OnDisable()
    {
        BtnErrorOK.onClick.RemoveListener(OnClickBtnErrorOK);
        BtnReigster.onClick.RemoveListener(OnClickBtnRegister);
        BtnBackToLogin.onClick.RemoveListener(OnClickBtnBackToLogin);
    }

    public void OnClickBtnRegister()
    {

    }

    public void OnClickBtnBackToLogin()
    {
        LoginPage.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickBtnErrorOK()
    {
        ErrorPanel.SetActive(false);
    }
}
