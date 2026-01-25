using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AsyncOperation _loadPlaySceneAsync;
    private void Awake()
    {
        Application.targetFrameRate = 120;
    }
    private void Start()
    {
        _loadPlaySceneAsync = SceneManager.LoadSceneAsync(1);
        _loadPlaySceneAsync.allowSceneActivation = false;
    }
    public void OnPlayClickHandler()
    {
        _loadPlaySceneAsync.allowSceneActivation = true;
    }
}
