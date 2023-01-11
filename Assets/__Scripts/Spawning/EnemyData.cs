using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float timeBtwAttacks;
    [SerializeField] private float attackRange;

    public float MoveSpeed => moveSpeed;
    public float Damage => damage;
    public float TimeBtwAttacks => timeBtwAttacks;
    public float AttackRange => attackRange;
}