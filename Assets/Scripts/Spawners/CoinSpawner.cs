using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class CoinSpawner : Spawner
{
    [Header("Coin Settings")] [SerializeField]
    private int _startCoinCount = 5;

    [SerializeField] private float _spawnTime = 5;

    private readonly string _prefabPath = $"Prefabs{Path.DirectorySeparatorChar}Currency{Path.DirectorySeparatorChar}";

    private Coroutine _spawnCoroutine;

    protected override void OnValidate()
    {
        base.OnValidate();

        if (_startCoinCount < 0)
            _startCoinCount = 0;

        if (_spawnTime < 0)
            _spawnTime = 0;
    }

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
            for (int i = 0; i < _startCoinCount; i++)
                Spawn(_prefabPath);
    }

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void OnDisable()
    {
        if (PhotonNetwork.IsMasterClient)
            StopCoroutine(_spawnCoroutine);
    }

    private IEnumerator SpawnCoroutine()
    {
        for (;;)
        {
            yield return new WaitForSeconds(_spawnTime);
            Spawn(_prefabPath);
        }
    }
}