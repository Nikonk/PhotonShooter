using System;
using Photon.Pun;
using UnityEngine;

public class ShootingOnMove : MonoBehaviour
{
    [SerializeField] private ProjectilePool _projectilePool;
    [SerializeField] private float _force;

    private Vector2 _shootDirection = new(1, 0);
    private Vector2 _shootPosition;

    private PlayerInput _playerInput;

    private PhotonView _photonView;

    public event Action OnShoot;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        _playerInput = new PlayerInput();

        if (_photonView.IsMine)
            _playerInput.Player.Shoot.performed += ctx => Shoot();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Update()
    {
        if (_photonView.AmOwner == false)
            return;

        if (_photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            return;

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

        var spawnShoot = _projectilePool.Pull(_shootPosition, Quaternion.Euler(0, 0, angle));
        spawnShoot.GetComponent<Rigidbody2D>().AddForce(_shootDirection * _force, ForceMode2D.Force);
    }
}