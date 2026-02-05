using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Image _shade; 
    private AsyncOperation _loadPlaySceneAsync;
    private void Awake()
    {
        Application.targetFrameRate = 120;
    }
    private void Start()
    {
        _loadPlaySceneAsync = SceneManager.LoadSceneAsync(1);
        _loadPlaySceneAsync.allowSceneActivation = false;
        StartCoroutine(UnhadeRoutine());
        
    }
    public void OnPlayClickHandler()
    {
        StartCoroutine(ShadeRoutine());
    }
    private IEnumerator ShadeRoutine()
    {
        var clr = _shade.color;
        for(var i=0f;i<1f;i+=Time.deltaTime * 2)
        {
            clr.a = i;
            _shade.color = clr;
            yield return null;
        }
        _loadPlaySceneAsync.allowSceneActivation = true;
    }
    private IEnumerator UnhadeRoutine()
    {
        _shade.raycastTarget = true;
        var clr = _shade.color;
        for (var i = 0f; i < 1f; i += Time.deltaTime * 4)
        {
            clr.a = 1- i;
            _shade.color = clr;
            yield return null;

        }
        _shade.raycastTarget = false;
        clr.a = 0;
        _shade.color = clr;
    }
}
