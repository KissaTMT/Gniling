using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseHolder : MonoBehaviour
{
    [SerializeField] private Image _pausePanel;
    
    public void Init()
    {
        Unpause();
    }
    public void OnBackInTheGameClickHandler()
    {
        Unpause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        _pausePanel.gameObject.SetActive(true);
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        _pausePanel.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Unpause();
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus) Unpause();
        else Pause();
    }
}
