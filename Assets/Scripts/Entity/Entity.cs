using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Entity : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected Collider2D _selfCol;

    [SerializeField] protected HPBar _hpBar = null;

    [SerializeField] private float _hp = 10.0f;
    public float HP { get => _hp; protected set{_hp = value < 0 ? 0 : value;} }
    public bool Dead => HP <= 0;

    public float MaxHP {get; protected set;}

    protected virtual void Awake()
    {
        MaxHP = HP;
        _rb = GetComponent<Rigidbody2D>();
        _selfCol = GetComponent<Collider2D>();
    }

    public void Hit(float dmg)
    {
        OnHit(dmg);
    }

    protected abstract void OnHit(float dmg);

    protected void DealDamage(float dmg)
    {
        HP -= dmg;
        _hpBar?.UpdateBar(this);
    }
}
