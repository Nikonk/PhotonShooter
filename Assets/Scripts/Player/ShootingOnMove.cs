using System;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class ShootingOnMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _projectilePrefab;
    [SerializeField] private float _force;
    
    private readonly string _prefabPath = $"Prefabs{Path.DirectorySeparatorChar}Projectiles";

    private Vector2 _shootDirection = new Vector2(1, 0);
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
        
        string shootPrefabPath = _prefabPath + Path.DirectorySeparatorChar + _projectilePrefab.name;

        _shootPosition = (Vector2)transform.position + _shootDirection * 1.5f;

        float angle = Vector2.SignedAngle(Vector2.down, _shootDirection.normalized);

        var spawnShoot = PhotonNetwork.Instantiate(shootPrefabPath, _shootPosition, Quaternion.Euler(0, 0, angle)).GetComponent<Rigidbody2D>();
        spawnShoot.AddForce(_shootDirection * _force, ForceMode2D.Impulse);
    }
}