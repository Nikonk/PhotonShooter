using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxValue;

    private int _currentValue;

    public event Action<float> OnHealthChange;

    private void Awake()
    {
        _currentValue = _maxValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Damaging>(out Damaging damaging))
        {
            _currentValue -= damaging.Damage;

            float healthProcent = _currentValue > 0 ? (float)_currentValue / (float)_maxValue
                                                    : 0;

            if (healthProcent == 0 && TryGetComponent<Movement>(out Movement movement))
                movement.enabled = false;
            
            OnHealthChange?.Invoke(healthProcent);
        }
    }
}
