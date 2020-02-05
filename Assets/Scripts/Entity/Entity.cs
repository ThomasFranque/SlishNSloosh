using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Entity : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected Collider2D _selfCol;

    public float HP {get; protected set;}
    public bool Dead => HP <= 0;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _selfCol = GetComponent<Collider2D>();
    }

    public void Hit()
    {
        OnHit();
    }

    protected abstract void OnHit();
}
