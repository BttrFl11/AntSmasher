using UnityEngine;

[System.Serializable]
public struct WaveStage
{
    [SerializeField] private Enemy[] _enemyVariants;
    [SerializeField] private int _enemyCount;
    [SerializeField] private float _timeBtwSpawns;

    public int EnemyCount => _enemyCount;
    public Enemy[] EnemyVariants => _enemyVariants;
    public float TimeBtwSpawns => _timeBtwSpawns;
}
