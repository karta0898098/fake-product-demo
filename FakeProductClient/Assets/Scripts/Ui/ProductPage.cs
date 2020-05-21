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
        headers = new Dictionary<string, string>();
        headers.Add("accessToken", Datacenter.Instance.AccessToken);
        NetworkManager.Instance.Get<ResponseProducts, ResponeError>("/api/products", headers, SuccessGetProducts, FailedGetProducts);

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

        NetworkManager.Instance.Post<ResponseBuyProducts, ResponeError>("/api/products/" + id, null, headers, SuccessBuyProduct, FailedBuyProduct);
    }

    public void OnClickInfoButton()
    {
        InfoPanel.gameObject.SetActive(false);
    }

    public void SuccessGetProducts(ResponseProducts resp)
    {
        Error.gameObject.SetActive(false);
        foreach (var product in resp.Products)
        {
            var uiProduct = Instantiate(ProductTemplate, ProductPanel);
            uiProduct.gameObject.SetActive(true);
            uiProduct.Id = Convert.ToInt32(product.Id);
            uiProduct.Title.text = product.Name;
            uiProduct.Price.text = product.Price.ToString();
            storeProdcut.Enqueue(uiProduct);
        }
    }

    public void SuccessBuyProduct(ResponseBuyProducts resp)
    {
        InfoPanel.gameObject.SetActive(true);
        Info.text = string.Format("購買 {0} 成功 \n 交易時間:{1} \n 交易訂單:{2}", resp.Product.Name, resp.CreateAt, resp.RecordId);
        Datacenter.Instance.Money = resp.User.Money;
        Money.text = resp.User.Money.ToString();
        IsBusy = false;
    }

    public void FailedGetProducts(ResponeError resp)
    {
        Error.text = resp.Error;
        Error.gameObject.SetActive(true);
    }

    public void FailedBuyProduct(ResponeError resp)
    {
        Info.text = resp.Error;
        InfoPanel.gameObject.SetActive(true);
        IsBusy = false;
    }
}
