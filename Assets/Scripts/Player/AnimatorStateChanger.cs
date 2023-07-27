using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorStateChanger : MonoBehaviour
{
    private const string _VerticalParameter = "Vertical";
    private const string _HorizontalParameter = "Horizontal";
    private const string _SpeedParameter = "Speed";
    private const string _AttackParameter = "Attack";
    private const string _DamageParameter = "Damage";
    private const string _RandomParameter = "Random";
    private const string _DieParameter = "Dead";
    
    private Animator _animator;

    private Movement _movement;
    private ShootingOnMove _shootingOnMove;
    private Health _health;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _movement = GetComponentInParent<Movement>();
        _shootingOnMove = GetComponentInParent<ShootingOnMove>();
        _health = GetComponentInParent<Health>();
    }

    private void OnEnable()
    {
        _movement.OnMove += MoveAnimation;
        _shootingOnMove.OnShoot += ShootAnimation;
        _health.OnHealthChange += DamageAnimation;
        _health.OnHealthChange += DieAnimation;
    }

    private void OnDisable()
    {
        _movement.OnMove -= MoveAnimation;
        _shootingOnMove.OnShoot -= ShootAnimation;
        _health.OnHealthChange -= DamageAnimation;
        _health.OnHealthChange -= DieAnimation;
    }

    private void MoveAnimation(Vector2 direction)
    {
        if (direction.sqrMagnitude >= 0.01)
        {
            _animator.SetFloat(_VerticalParameter, direction.x);
            _animator.SetFloat(_HorizontalParameter, direction.y);
        }

        _animator.SetFloat(_SpeedParameter, direction.sqrMagnitude);
    }

    private void ShootAnimation()
    {
        _animator.SetTrigger(_AttackParameter);
    }

    private void DieAnimation(float value)
    {
        if (value != 0)
            return;
        
        _animator.SetFloat(_RandomParameter, Random.Range(0, 1));
        _animator.SetTrigger(_DieParameter);
    }

    private void DamageAnimation(float value)
    {
        if (value == 0)
            return;
        
        _animator.SetTrigger(_DamageParameter);
    }
}
