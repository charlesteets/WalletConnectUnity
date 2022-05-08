using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Unity.Models;
using WalletConnectSharp.Unity.Utils;

[RequireComponent(typeof(Button))]
public class SignInWithEthereumButton : BindableMonoBehavior
{
    public WCConnectionSettings sessionOptions;
    
    [BindComponent]
    private Button _button;
    
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private async void OnButtonClicked()
    {
        if (WC.IsConnected)
        {
            WC.Disconnect();
        }
        
        //First connect with walletconnect
        //(or continue if we're already connected)
        await WC.Connect(options: sessionOptions);
        
        //Sign the message
        var signature = await WC.SignMessage("This is a test!");
        
        //Verify signature
        Debug.Log(signature);
        
        //TODO Verify
        
        //TODO Propagate authentication
        
        //Disconnect
        WC.Disconnect();
    }
}
