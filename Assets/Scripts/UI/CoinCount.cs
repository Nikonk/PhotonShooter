using UnityEngine;
using TMPro;

public class CoinCount : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TMP_Text _coinCount;

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
