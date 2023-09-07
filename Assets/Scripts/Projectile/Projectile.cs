using System.IO;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    private readonly string _explosionPrefabPath = $"Prefabs{Path.DirectorySeparatorChar}Projectiles{Path.DirectorySeparatorChar}";

    [SerializeField] private ParticleSystem _explosionPrefab;

    private Transform _transform;
    private GameObject _gameObject;
    private ProjectilePool _pool;

    public Projectile Initialize(ProjectilePool pool)
    {
        _pool = pool;

        _transform = transform;
        _gameObject = gameObject;
        _gameObject.SetActive(false);

        return this;
    }

    public void Push()
    {
        _pool.Push(this);
    }

    public Projectile Pull(Vector2 position, Quaternion angle)
    {
        _transform.position = position;
        _transform.rotation = angle;

        SetActive(true);

        return this;
    }

    public void SetActive(bool value)
    {
        _gameObject.SetActive(value);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string explosionPrefabPath = _explosionPrefabPath + _explosionPrefab.name;
        PhotonNetwork.Instantiate(explosionPrefabPath, transform.position, Quaternion.identity);

        Push();
    }

}
