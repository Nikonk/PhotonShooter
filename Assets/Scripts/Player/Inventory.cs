using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnCoinCountChanged;

    public int CoinCount { get; private set; } = 0;

    public void AddCoin()
    {
        CoinCount++;

        OnCoinCountChanged?.Invoke();
    }
}
