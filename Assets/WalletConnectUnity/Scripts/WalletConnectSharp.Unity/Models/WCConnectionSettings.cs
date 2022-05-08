using System;
using WalletConnectSharp.Core.Models;

namespace WalletConnectSharp.Unity.Models
{
    [Serializable]
    public class WCConnectionSettings
    {
        public Wallets DeepLinkDefaultWallet;
        public int connectSessionRetryCount = 3;
        public string customBridgeUrl;
        public ClientMeta dAppData;
    }
}