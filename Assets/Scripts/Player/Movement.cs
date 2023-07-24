using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    
    private PlayerInput _playerInput;

    public event Action<Vector2> OnMove;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _playerInput = new PlayerInput();

        _playerInput.Player.Move.performed += ctx => Move();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Update()
    {
        _direction = _playerInput.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Move()
    {
        OnMove?.Invoke(_direction);

        if (_direction.sqrMagnitude < 0.01)
            return;

        _rigidbody.MovePosition((Vector2)transform.position + _direction * (_speed * Time.deltaTime));
    }
}
