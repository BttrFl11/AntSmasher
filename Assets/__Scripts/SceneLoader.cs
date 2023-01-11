using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float _transitionTime;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator Load(int index)
    {
        _animator.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(index);
    }

    public void Restart()
    {
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNext()
    {
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex + 1));
    }
}
