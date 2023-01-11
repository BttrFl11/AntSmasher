using UnityEngine;

public class LevelLoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _gameOverPanel;

    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();

        _winPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameStateController.OnLevelCompleted += ShowWinPanel;
        GameStateController.OnGameOver += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        GameStateController.OnLevelCompleted -= ShowWinPanel;
        GameStateController.OnGameOver -= ShowGameOverPanel;
    }

    private void ShowWinPanel()
    {
        _winPanel.SetActive(true);
    }

    private void ShowGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        //

        _sceneLoader.LoadNext();
    }
}