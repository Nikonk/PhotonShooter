using System.IO;
using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionPrefab;

    private readonly string _explosionPrefabPath = $"Prefabs{Path.DirectorySeparatorChar}Projectiles{Path.DirectorySeparatorChar}";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string explosionPrefabPath = _explosionPrefabPath + _explosionPrefab.name;
        PhotonNetwork.Instantiate(explosionPrefabPath, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
