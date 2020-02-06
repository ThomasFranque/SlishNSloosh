using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private bool _useOffset = true;
    private Transform _target = null;
    private Vector3 _offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position - _offset;
    }

    public void UpdateBarValues(Transform target)
    {
        _target = target;
        UpdateOffset();
    }

    // Called in EntityProperties ScriptableObject SendMessage
    public void UpdateOffset()
    {
        if (_useOffset)
            _offset = _target.transform.position - transform.position;
        else
            _offset = Vector3.zero;
    }
}
