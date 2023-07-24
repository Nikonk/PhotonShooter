using UnityEngine;

public abstract class Damaging : MonoBehaviour
{
    [SerializeField] private int _damage;

    public int Damage => _damage;
}
