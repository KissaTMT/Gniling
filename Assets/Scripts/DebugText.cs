using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        DebugHandler(Player.instance.CurrentPoint.ToString());
    }
    private void DebugHandler(string text)
    {
        _text.text = text;
    }
}
