using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
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
        StartCoroutine(WaitForAdsRoutine());
    }
    private void OnDisable()
    {
        _gniling.OnDeath -= ShowGameOverPanel;
    }
    private IEnumerator WaitForAdsRoutine()
    {
        yield return new WaitForSeconds(2);
        YG2.InterstitialAdvShow();
    }
}
