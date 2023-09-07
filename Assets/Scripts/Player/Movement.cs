using System;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _controllable = true;

    private Health _health;

    private PlayerInput _playerInput;

    private PhotonView _photonView;

    public event Action<Vector2> OnMove;

    private void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += ctx => Move();

        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _health.OnHealthChange += OnDead;
    }

    private void Update()
    {
        if (_photonView.AmOwner == false)
            return;

        if (_photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            return;

        if (_controllable == false)
            return;

        _direction = _playerInput.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (_photonView.IsMine && _controllable)
            Move();
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _health.OnHealthChange -= OnDead;
    }

    private void OnDead(float healthProcent)
    {
        if (healthProcent != 0)
            return;

        _controllable = false;
    }

    private void Move()
    {
        OnMove?.Invoke(_direction);

        if (_direction.sqrMagnitude < 0.01)
            return;

        _rigidbody.MovePosition((Vector2)transform.position + _direction * (_speed * Time.deltaTime));
    }
}