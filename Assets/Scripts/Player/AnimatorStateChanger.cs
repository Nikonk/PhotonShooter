using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorStateChanger : MonoBehaviour
{
    private Animator _animator;

    private const string _VerticalParameter = "Vertical";
    private const string _HorizontalParameter = "Horizontal";
    private const string _SpeedParameter = "Speed";
    private const string _AttackParameter = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetComponentInParent<Movement>().OnMove += MoveAnimation;
        GetComponentInParent<ShootingOnMove>().OnShoot += ShootAnimation;
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
}
