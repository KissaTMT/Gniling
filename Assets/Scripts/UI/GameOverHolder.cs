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
    [SerializeField] private Image _shade;
    private AsyncOperation _loadMenuSceneAsync;

    private PlayerGnilingBrian _player;
    private Gniling _gniling;

    [Inject]
    public void Construct(PlayerGnilingBrian player)
    {
        _player = player;
        
    }
    public void Init()
    {
        _gniling = _player.Gniling;
        _gniling.OnDeath += ShowGameOverPanel;
        _panel.gameObject.SetActive(false);
    }

    public void OnBackToMenuClickHandler()
    {
        StartCoroutine(ShadeRoutine());
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
    private IEnumerator ShadeRoutine()
    {
        var clr = _shade.color;
        for (var i = 0f; i < 1f; i += Time.deltaTime * 2)
        {
            clr.a = i;
            _shade.color = clr;
            yield return null;
        }
        _loadMenuSceneAsync.allowSceneActivation = true;
    }
}
