using System.IO;
using UnityEngine;
using Photon.Pun;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPrefab;

    [Header("Spawn Bounds")] 
    [SerializeField] private float _upBound;
    [SerializeField] private float _rightBound;
    [SerializeField] private float _bottomBound;
    [SerializeField] private float _leftBound;
    
    private Coroutine _spawnCoroutine;

    protected virtual void OnValidate()
    {
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

    protected void Spawn(string prefabFolderPath)
    {
        Vector2 randomPosition = new Vector2(Random.Range(_leftBound, _rightBound),
                                             Random.Range(_bottomBound, _upBound));

        string prefabPath = prefabFolderPath + _spawnPrefab.name;

        PhotonNetwork.Instantiate(prefabPath, randomPosition, Quaternion.identity);
    }
}