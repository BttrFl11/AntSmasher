using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;

    protected float _health;

    protected virtual void Awake()
    {
        _health = _maxHealth;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            Die();
        }
    }
}
