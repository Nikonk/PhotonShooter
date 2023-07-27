using UnityEngine;

public class HealthRenderer : MonoBehaviour
{
    private const string _healthName = "_Health";
    
    private Renderer _healthRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private Health _health;

    private void Awake()
    {
        _healthRenderer = GetComponent<Renderer>();
        _health = GetComponentInParent<Health>();

        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        _health.OnHealthChange += ChangeValue;
    }

    private void OnDisable()
    {
        _health.OnHealthChange -= ChangeValue;
    }

    private void ChangeValue(float value)
    {
        _healthRenderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetFloat(_healthName, value);
        _healthRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
