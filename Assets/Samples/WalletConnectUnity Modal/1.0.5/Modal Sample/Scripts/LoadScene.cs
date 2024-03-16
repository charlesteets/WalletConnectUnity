using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using WalletConnectUnity.Core;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private Scene scene;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        print($"log] post-wait: {WalletConnect.Instance.SignClient.Session.Context}");
        print($"log] post-wait: {WalletConnect.Instance.SignClient.Session.GetHashCode()}");
        foreach (var thing in WalletConnect.Instance.SignClient.Session.Keys)
            print($"log] post-wait: {thing}");
    }

    public void Load(string sceneName)
    {
        print($"log] post-wait: {WalletConnect.Instance.SignClient.Session.Context}");
        print($"log] post-wait: {WalletConnect.Instance.SignClient.Session.GetHashCode()}");
        foreach (var thing in WalletConnect.Instance.SignClient.Session.Keys)
            print($"log] post-wait: {thing}");
        SceneManager.LoadScene(sceneName);
    }
    public void Load()
    {
        SceneManager.LoadScene(scene.name);
    }
}