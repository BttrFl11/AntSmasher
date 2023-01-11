using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private void Awake()
    {
        _panel.SetActive(true);
    }

    public void OnClick()
    {
        GameStateController.ChangeState(GameState.Start);

        _panel.SetActive(false);
    }
}