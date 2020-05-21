using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datacenter : MonoBehaviour
{
    private static Datacenter instance = null;
    public static Datacenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("Datacenter").AddComponent(typeof(Datacenter)) as Datacenter;
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public string AccessToken;
    public string UserName;
    public long Money;
}
