using System;
using UnityEngine;

public class ShootingOnMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _projectilePrefab;
    [SerializeField] private float _force;

    private Vector2 _shootDirection = new Vector2(1, 0);
    private Vector2 _shootPosition;

    private PlayerInput _playerInput;

    public event Action OnShoot;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Shoot.performed += ctx => Shoot();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Update()
    {
        if (_playerInput.Player.Move.ReadValue<Vector2>().sqrMagnitude < 0.01f)
            return;

        _shootDirection = _playerInput.Player.Move.ReadValue<Vector2>().normalized;        
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Shoot()
    {
        OnShoot?.Invoke();

        _shootPosition = (Vector2)transform.position + _shootDirection * 1.5f;

        float angle = Vector2.SignedAngle(Vector2.down, _shootDirection.normalized);

        Rigidbody2D spawnShoot = Instantiate(_projectilePrefab, _shootPosition, Quaternion.Euler(0, 0, angle));
        spawnShoot.AddForce(_shootDirection * _force, ForceMode2D.Impulse);
    }
}
