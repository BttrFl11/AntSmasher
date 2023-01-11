using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Damageable
{
    [SerializeField] private float _healthAnimationSpeed;

    [Header("UI")]
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Image _healthFill;
    [SerializeField] private Image _healthFillTrail;

    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();

        _healthBar.SetActive(false);
    }

    private void Update()
    {
        AnimateHealthTrail();
    }

    private void AnimateHealthTrail()
    {
        _healthFillTrail.fillAmount = Mathf.Lerp(_healthFillTrail.fillAmount, _healthFill.fillAmount, _healthAnimationSpeed);
    }

    protected override void Die()
    {
        // Turn off health ui
        _healthBar.SetActive(false);

        // Play death animation
        _animator.SetBool("IsDead", true);

        // Show game over ui
    }

    public override void TakeDamage(float damage)
    {
        _health -= damage;

        _healthBar.SetActive(true);
        _healthFill.fillAmount = _health / _maxHealth;

        if (_health <= 0)
        {
            Die();
        }
    }
}
