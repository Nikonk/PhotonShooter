using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
