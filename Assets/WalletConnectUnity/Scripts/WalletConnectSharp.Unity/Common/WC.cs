using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Unity;
using WalletConnectSharp.Unity.Models;
using Object = UnityEngine.Object;

public static class WC
{
    private static WalletConnect _instance;
    public static WalletConnect Instance
    {
        get
        {
            if (_instance == null)
                _instance = Object.FindObjectOfType<WalletConnect>();

            return _instance;
        }
        private set
        {
            if (_instance != null)
            {
                if (_instance.Connected)
                    _instance.Session.Disconnect();
            }
            
            _instance = value;
        }
    } 
    
    public static WalletConnectUnitySession Session
    {
        get
        {
            if (Instance != null)
                return Instance.Session;
            return null;
        }  
    }

    public static bool IsConnected
    {
        get
        {
            var session = Session;
            return session != null && session.Connected;
        }
    }

    public static async Task<WalletConnectUnitySession> Connect(Wallets openWallet = Wallets.None, WCConnectionSettings options = default)
    {
        if (Session != null)
        {
            throw new Exception("Session already live, try using GetOrConnect()");
        }

        if (options == default)
        {
            options = new WCConnectionSettings();
        }

        if (openWallet != Wallets.None)
        {
            options.DeepLinkDefaultWallet = openWallet;
        }
        
        var wcObject = new GameObject();
        wcObject.name = "WalletConnect";
        var wc = wcObject.AddComponent<WalletConnect>();

        //set options
        wc.AppData = options.dAppData;
        wc.customBridgeUrl = options.customBridgeUrl;
        wc.DefaultWallet = options.DeepLinkDefaultWallet;
        /*wc.connectOnAwake = options.connectOnAwake;
        wc.connectOnStart = options.connectOnStart;
        wc.autoSaveAndResume = options.autoSaveAndResume;*/
        wc.connectSessionRetryCount = options.connectSessionRetryCount;
        wc.connectOnStart = false;
        wc.connectOnAwake = false;
        wc.createNewSessionOnSessionDisconnect = false;
        //wc.createNewSessionOnSessionDisconnect = options.createNewSessionOnSessionDisconnect;

        wc.Connect();

        await wc.WaitUntilReady();


#if UNITY_ANDROID
        wc.OpenDeepLink();
#elif UNITY_IOS
        if (wc.DefaultWallet != Wallets.None)
            wc.OpenDeepLink();
        else
            throw new Exception("You must target a specific Wallet by passing an openWallet parameter");
#else
        throw new Exception("Desktop not supported currently");
#endif

        await wc.WaitForConnected();

        Instance = wc;

        return wc.Session;
    }
    
    public static async Task<WalletConnectUnitySession> GetOrConnect(Wallets openWallet = Wallets.None, WCConnectionSettings options = default)
    {
        var session = Session;
        if (session != null && session.Connected)
            return session;
        else if (session != null && session.Connecting)
        {
            await Instance.WaitForConnected();
            
            return session;
        }

        return await Connect(openWallet, options);
    }

    public static async void EnsureConnected()
    {
        var session = Session;
        
        if (session == null || (!session.Connected && !session.Connecting))
            throw new IOException("Session not connected, use WC.Connect() first");
        
        if (session.Connecting)
        {
            await Instance.WaitForConnected();
        }
    }

    public static async Task<string> SignMessage(string message)
    {
        var session = Session;

        EnsureConnected();

        return await session.EthSign(session.Accounts[0], message);
    }
    
    
    public static async Task<string> PersonalSignMessage(string message)
    {
        var session = Session;

        EnsureConnected();

        return await session.EthPersonalSign(session.Accounts[0], message);
    }
    
    public static async Task<string> SignTypedData<T>(T message, EIP712Domain domain)
    {
        var session = Session;

        EnsureConnected();

        return await session.EthSignTypedData(session.Accounts[0], message, domain);
    }
    
    
    public static async Task<string> SignTransaction(params TransactionData[] transactionData)
    {
        var session = Session;

        EnsureConnected();

        return await session.EthSignTransaction(transactionData);
    }
    
    public static async Task<string> SendTransaction(params TransactionData[] transactionData)
    {
        var session = Session;

        EnsureConnected();

        return await session.EthSendTransaction(transactionData);
    }
    
    public static async void Disconnect()
    {
        var current = Instance;
        if (current == null)
            throw new IOException("Session not connected, use WC.Connect() first");
        
        var session = current.Session;

        EnsureConnected();

        await session.Disconnect();
        
        Object.Destroy(current);
    }
}