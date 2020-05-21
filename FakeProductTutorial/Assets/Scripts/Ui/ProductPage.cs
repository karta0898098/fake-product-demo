using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductPage : MonoBehaviour
{

    public Product ProductTemplate;
    public Transform ProductPanel;

    public Text Name;
    public Text Money;
    public Text Error;

    public GameObject InfoPanel;
    public Text Info;
    public Button BtnInfo;

    private Dictionary<string, string> headers;
    private Queue<Product> storeProdcut;

    public bool IsBusy;

    public void OnEnable()
    {
        storeProdcut = new Queue<Product>();


        Name.text = Datacenter.Instance.UserName;
        Money.text = Datacenter.Instance.Money.ToString();

        Product.OnRequestBuyItem += HandlerRequestBuy;
        BtnInfo.onClick.AddListener(OnClickInfoButton);
    }

    public void OnDisable()
    {
        BtnInfo.onClick.RemoveListener(OnClickInfoButton);
        Product.OnRequestBuyItem -= HandlerRequestBuy;

        var preDestorys = storeProdcut.Count;
        for (int i = 0; i < preDestorys; i++)
        {
            var preDestory = storeProdcut.Dequeue();
            Destroy(preDestory.gameObject);
        }
    }

    public void HandlerRequestBuy(int id)
    {
        if (IsBusy)
        {
            return;
        }
    }

    public void OnClickInfoButton()
    {
        InfoPanel.gameObject.SetActive(false);
    }

    public void SuccessGetProducts(ResponseProducts resp)
    {


    }

    public void SuccessBuyProduct(ResponseBuyProducts resp)
    {

    }

    public void FailedGetProducts(ResponeError resp)
    {

    }

    public void FailedBuyProduct(ResponeError resp)
    {

    }
}
