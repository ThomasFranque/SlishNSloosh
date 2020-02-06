using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitText : MonoBehaviour
{
    [SerializeField] private float _risingSpeed = 20.0f;
    [SerializeField] private float _yOffset = 26.0f;
    [SerializeField] private float _autoDestructInSecs = .6f;
    private Vector3 _newPos;

    private void Awake()
    {
        _newPos = transform.position;
        _newPos.y += _yOffset;

        Destroy(gameObject, _autoDestructInSecs);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _newPos, Time.deltaTime * _risingSpeed);
    }
}
