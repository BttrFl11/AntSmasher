using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyWave")]
public class EnemyWaveSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private WaveStage[] _waveStages;

    public WaveStage[] WaveStages => _waveStages;
    public string Name => _name;
}