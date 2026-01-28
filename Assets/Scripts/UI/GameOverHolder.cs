using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverHolder : MonoBehaviour
{
    [SerializeField] private Image _panel;
    private AsyncOperation _loadMenuSceneAsync;

    private Gniling _gniling;

    [Inject]
    public void Construct(PlayerGnilingBrian player)
    {
        _gniling = player.Gniling;
        _gniling.OnDeath += ShowGameOverPanel;
    }
    public void Init()
    {
        _panel.gameObject.SetActive(false);
    }

    public void OnBackToMenuClickHandler()
    {
        _loadMenuSceneAsync.allowSceneActivation = true;
    }
    private void ShowGameOverPanel()
    {
        _panel.gameObject.SetActive(true);
        _loadMenuSceneAsync = SceneManager.LoadSceneAsync(0);
        _loadMenuSceneAsync.allowSceneActivation = false;
    }
    private void OnDisable()
    {
        _gniling.OnDeath -= ShowGameOverPanel;
    }
}
