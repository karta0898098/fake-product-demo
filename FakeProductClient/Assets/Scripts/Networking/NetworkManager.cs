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

    public void Get<T, K>(string url, Dictionary<string, string> headers, Action<T> success, Action<K> failed)
    {
        StartCoroutine(DoGET(url, headers, success, failed));
    }

    public void Post<T, K>(string url, object data, Dictionary<string, string> headers, Action<T> success, Action<K> failed)
    {
        StartCoroutine(DoPOST(url, data, headers, success, failed));
    }


    private UploadHandlerRaw JsonUploadHandler(object data)
    {
        if (data == null)
        {
            return null;
        }

        var json = JsonConvert.SerializeObject(data);
        var raw = System.Text.Encoding.UTF8.GetBytes(json);

        var uploader = new UploadHandlerRaw(raw);

        uploader.contentType = "application/json";
        return uploader;
    }



    IEnumerator DoGET<T, K>(string url, Dictionary<string, string> headers, Action<T> success, Action<K> failed)
    {
        var currentURL = string.Format("{0}{1}", Host, url);

        var request = UnityWebRequest.Get(currentURL);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }

        yield return request.SendWebRequest();

        if (request.isHttpError)
        {
            K result = JsonConvert.DeserializeObject<K>(request.downloadHandler.text);
            failed?.Invoke(result);
        }

        if (request.responseCode == 200)
        {
            T result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            success?.Invoke(result);
        }

    }

    IEnumerator DoPOST<T, K>(string url, object data, Dictionary<string, string> headers, Action<T> success, Action<K> failed)
    {
        var currentURL = string.Format("{0}{1}", Host, url);


        var uploader = JsonUploadHandler(data);
        var downloader = new DownloadHandlerBuffer();

        var request = new UnityWebRequest(currentURL, UnityWebRequest.kHttpVerbPOST);
        request.uploadHandler = uploader;
        request.downloadHandler = downloader;
        request.SetRequestHeader("Accept", "application/json");
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }


        yield return request.SendWebRequest();

        if (request.isHttpError)
        {
            K result = JsonConvert.DeserializeObject<K>(request.downloadHandler.text);
            failed?.Invoke(result);
        }

        if (request.responseCode == 200)
        {
            T result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            success?.Invoke(result);
        }
    }
}


