using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _startCoinCount = 5;
    [SerializeField] private float _spawnTime = 5;

    [Header("Spawn Bounds")] 
    [SerializeField] private float _upBound;
    [SerializeField] private float _rightBound;
    [SerializeField] private float _bottomBound;
    [SerializeField] private float _leftBound;
    
    private Coroutine _spawnCoroutine;

    private void OnValidate()
    {
        if (_startCoinCount < 0)
            _startCoinCount = 0;
        
        if (_spawnTime < 0)
            _spawnTime = 0;

        float temp;

        if (_bottomBound > _upBound)
        {
            temp = _upBound;
            _upBound = _bottomBound;
            _bottomBound = temp;
        }

        if (_leftBound > _rightBound)
        {
            temp = _rightBound;
            _rightBound = _leftBound;
            _leftBound = temp;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < _startCoinCount; i++)
            SpawnCoin();
    }

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_spawnCoroutine);
    }

    private void SpawnCoin()
    {
        float xCoordinates = Random.Range(_leftBound, _rightBound);
        float yCoordinates = Random.Range(_bottomBound, _upBound);

        Instantiate(_coinPrefab, new Vector2(xCoordinates, yCoordinates), Quaternion.identity);
    }

    private IEnumerator SpawnCoroutine()
    {
        for (;;)
        {
            yield return new WaitForSeconds(_spawnTime);
            SpawnCoin();
        }
    }
}
