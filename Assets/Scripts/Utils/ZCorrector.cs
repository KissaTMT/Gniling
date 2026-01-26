using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ZCorrector : MonoBehaviour
{
    private Transform _transform;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    private void Update()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.y);
    }
}