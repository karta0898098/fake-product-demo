using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour
{


    public enum ServerLocation
    {
        Debug,
        Release
    }

    public ServerLocation serverLocation;

    private string Host = "http://127.0.0.1:8080";

    private static NetworkManager instance = null;
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("NetworkManager").AddComponent(typeof(NetworkManager)) as NetworkManager;
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        if (serverLocation == ServerLocation.Release)
        {

        }
        else
        {
            Host = "http://127.0.0.1:8080";
        }
    }
}


