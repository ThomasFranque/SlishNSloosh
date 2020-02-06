using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private bool _useOffset = true;
    private Vector3 _offset;

    private void Awake()
    {
        if (_useOffset)
            _offset = _target.transform.position - transform.position;
        else
            _offset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position - _offset;
    }
}
