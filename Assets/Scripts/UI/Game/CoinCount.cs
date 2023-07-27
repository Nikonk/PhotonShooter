using System;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CoinCount : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinCount;
    
    private Inventory _inventory;

    private void Awake()
    {
        var inventories = GameObject.FindObjectsOfType<Inventory>();

        foreach (Inventory inventory in inventories)
            if (inventory.gameObject.GetComponent<PhotonView>().IsMine)
            {
                _inventory = inventory;
                break;
            }
    }

    private void OnEnable()
    {
        _inventory.OnCoinCountChanged += ChangeCoinCount;
    }

    private void OnDisable()
    {
        _inventory.OnCoinCountChanged -= ChangeCoinCount;
    }

    private void ChangeCoinCount()
    {
        _coinCount.SetText(_inventory.CoinCount.ToString());
    }
}
