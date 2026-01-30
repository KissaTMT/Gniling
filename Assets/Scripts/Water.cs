using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class Water : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animationSpeed = 1;
    private SpriteShapeController _controller;

    private void Awake()
    {
        _controller = GetComponent<SpriteShapeController>();
    }
    private void Start()
    {
        StartCoroutine(AnimateEdgeRoutine());
    }
    private IEnumerator AnimateEdgeRoutine()
    {
        while (true)
        {
            for (var i = 0; i < _sprites.Length; i++)
            {
                _controller.spriteShape.angleRanges[0].sprites[0] = _sprites[i];
                yield return new WaitForSeconds(1 / (16f * _animationSpeed));
            }
        }
    }
}
