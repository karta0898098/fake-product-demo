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
        if (IsBusy)
        {
            return;
        }


        IsBusy = true;
        var data = new RequestLogin();
        data.Account = Account.text;
        data.Password = Password.text;
        NetworkManager.Instance.Post<ResponseLogin, ResponeError>("/api/login", data, null,
        (resp) =>
        {
            Datacenter.Instance.AccessToken = resp.AccessToken;
            Datacenter.Instance.UserName = resp.User.Name;
            Datacenter.Instance.Money = resp.User.Money;
            ProdctPage.SetActive(true);
            gameObject.SetActive(false);
            IsBusy = false;
        },
        (resp) =>
        {
            ErrorText.text = resp.Error;
            ErrorPanel.SetActive(true);
            IsBusy = false;
        });
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
