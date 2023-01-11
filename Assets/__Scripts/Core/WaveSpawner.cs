using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemyWaveSO _wave;
    [SerializeField] private float _timeBtwWaves;
    [SerializeField] private float _timeBtwStages;
    [SerializeField] private Vector2 _enemySpawnStartPosXZ, _enemySpawnEndPosXZ;

    private int _enemiesAlive;

    public int EnemiesAlive => _enemiesAlive;

    public static event Action OnStageEnded;
    public static event Action OnStageStart;
    public static event Action OnAllWavesEnded;

    private void OnEnable()
    {
        GameStateController.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameStateController.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        for (int j = 0; j < _wave.WaveStages.Length; j++)
        {
            yield return new WaitForSeconds(_timeBtwStages);

            OnStageStart?.Invoke();

            var stage = _wave.WaveStages[j];
            for (int l = 0; l < stage.EnemyCount; l++)
            {
                var pos = GetRandomSpawnPosition();
                var enemy = GetRangomEnemy(stage.EnemyVariants);
                SpawnSingleEnemy(enemy, pos);

                yield return new WaitForSeconds(stage.TimeBtwSpawns);
            }

            yield return new WaitUntil(() => EnemiesAlive == 0);

            OnStageEnded?.Invoke();
        }

        yield return new WaitUntil(() => EnemiesAlive == 0);

        OnAllWavesEnded?.Invoke();

        yield return null;
    }

    private void OnEnemyDied()
    {
        _enemiesAlive--;
    }

    private Enemy GetRangomEnemy(Enemy[] enemyVariants)
    {
        var randEnemy = enemyVariants[Range(0, enemyVariants.Length)];
        return randEnemy;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var x = Range(_enemySpawnStartPosXZ.x, _enemySpawnEndPosXZ.x);
        var z = Range(_enemySpawnStartPosXZ.y, _enemySpawnEndPosXZ.y);
        return new Vector3(x, 0, z);
    }

    private void SpawnSingleEnemy(Enemy enemy, Vector3 pos)
    {
        var spawnedEnemy = Instantiate(enemy, pos, Quaternion.Euler(0, -90, 0));
        spawnedEnemy.OnDied += OnEnemyDied;
        _enemiesAlive++;
    }
}