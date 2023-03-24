using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button host;
    [SerializeField] private Button client;
    [SerializeField] private Button start;

    [SerializeField] private ZSpawning2 spawner;

    private void Awake()
    {
        host.onClick.AddListener(() => { NetworkManager.Singleton.StartHost();});
        client.onClick.AddListener(() => { NetworkManager.Singleton.StartClient();});
        start.onClick.AddListener(() => { spawner.Initialize(); LevelStats.main.Initialize();});
    }
}
