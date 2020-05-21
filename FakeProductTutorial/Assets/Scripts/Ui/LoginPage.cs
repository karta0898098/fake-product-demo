using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : MonoBehaviour
{
    public InputField Account;
    public InputField Password;
    public Button BtnLogin;
    public Button BtnReigster;
    public Button BtnErrorOK;

    public GameObject ErrorPanel;
    public Text ErrorText;

    public GameObject RegisterPage;
    public GameObject ProdctPage;

    public bool IsBusy;

    public void OnEnable()
    {
        BtnLogin.onClick.AddListener(OnClickBtnLogin);
        BtnReigster.onClick.AddListener(OnClickBtnRegister);
        BtnErrorOK.onClick.AddListener(OnClickBtnErrorOK);
    }

    public void OnDisable()
    {
        BtnErrorOK.onClick.RemoveListener(OnClickBtnErrorOK);
        BtnReigster.onClick.RemoveListener(OnClickBtnRegister);
        BtnLogin.onClick.RemoveListener(OnClickBtnLogin);
    }

    public void OnClickBtnLogin()
    {

    }

    public void OnClickBtnRegister()
    {
        gameObject.SetActive(false);
        RegisterPage.SetActive(true);
    }

    public void OnClickBtnErrorOK()
    {
        ErrorPanel.SetActive(false);
    }
}
