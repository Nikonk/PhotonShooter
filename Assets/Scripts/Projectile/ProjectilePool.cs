using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    private readonly string _prefabPath = $"Prefabs{Path.DirectorySeparatorChar}Projectiles{Path.DirectorySeparatorChar}";

    [SerializeField] private Projectile _projectilePrefab;

    private Queue<Projectile> _spawnQueue = new();

    public Projectile Pull(Vector2 spawnPoint, Quaternion angle)
    {
        if (_spawnQueue.Count == 0)
        {
            string shootPrefabPath = _prefabPath + _projectilePrefab.name;
            PhotonNetwork.Instantiate(shootPrefabPath, spawnPoint, angle).GetComponent<Projectile>().Initialize(this).Push();
        }

        return _spawnQueue.Dequeue().Pull(spawnPoint, angle);
    }

    public void Push(Projectile spawnableObject)
    {
        spawnableObject.SetActive(false);
        _spawnQueue.Enqueue(spawnableObject);
    }
}