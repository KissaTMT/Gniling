using TMPro;
using UnityEngine;
using YG;

public class TextTranslator : MonoBehaviour
{
    [SerializeField] private string _ru;
    [SerializeField] private string _en;

    private TextMeshProUGUI _tmp;
    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _tmp.text = YG2.envir.language == "ru" ? _ru : _en;
    }
}