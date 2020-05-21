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
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        var data = new RequestRegister();
        data.Account = Account.text;
        data.Password = Password.text;
        data.Name = Name.text;
        NetworkManager.Instance.Post<ResponseRegister, ResponeError>("/api/register", data, null,
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
