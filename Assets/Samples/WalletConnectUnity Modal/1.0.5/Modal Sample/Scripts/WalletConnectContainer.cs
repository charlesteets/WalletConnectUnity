using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DriveWorld.Client
{
    public class WalletConnectContainer : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;
        private GameObject instance;
        private void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = Instantiate(prefab, transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
