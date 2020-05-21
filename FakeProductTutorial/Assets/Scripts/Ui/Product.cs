using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public Text Title;
    public Text Price;
    public Text Description;
    public int Id;

    public Button BtnBuy;

    public static event Action<int> OnRequestBuyItem;

    public void Start()
    {

    }

    private void OnEnable()
    {
        BtnBuy.onClick.AddListener(RequestBuy);
    }

    private void OnDisable()
    {
        BtnBuy.onClick.RemoveListener(RequestBuy);
    }

    public void SuccessGetProducts(ResponseProduct resp)
    {
        Description.text = resp.Description;
    }

    public void FailedGetProducts(ResponeError resp)
    {
        //TODO Error
    }

    public void RequestBuy()
    {
        OnRequestBuyItem?.Invoke(Id);
    }
}
