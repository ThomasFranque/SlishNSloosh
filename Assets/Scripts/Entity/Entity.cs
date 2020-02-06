using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityProperties _properties = null;
    private Rigidbody2D _rb;
    private Collider2D _selfCol;

    protected EntityVisuals _entityVisuals;
    private float _hp;
    protected HPBar _hpBar;

    public float HP { get => _hp; protected set { _hp = value < 0 ? 0 : value; } }
    public bool Dead => HP <= 0;
    protected bool Killed { get; private set; }
    public float TotalHP => _properties.TotalHP;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _selfCol = GetComponent<Collider2D>();
        _entityVisuals = gameObject.AddComponent<EntityVisuals>();

        if (_properties == null) Debug.LogError($"{name} does not have Entity properties!");
        if (_hpBar == null) _hpBar = _properties.NewHealthBar(transform);
        _hp = TotalHP;

    }

    public void Hit(float dmg, Vector2 hitDirection, float knockBackIntensity)
    {
        if (Killed) return;
        OnHit(dmg, hitDirection, knockBackIntensity);
    }

    private void OnDeath(float finalDeathDmg)
    {
        if (Killed) return;
        Killed = true;
        OnKill(finalDeathDmg);
    }

    protected virtual void OnKill(float finalDeathDmg)
    {
        _entityVisuals?.FlashSprite(Color.white, 0.3f);
        _entityVisuals?.Slowtime(0.1f, 1.0f * (1 - (finalDeathDmg / TotalHP)));
    }

    protected abstract void OnHit(float dmg, Vector2 hitDirection, float knockBackIntensity);

    protected void DealDamage(float dmg)
    {
        HP -= dmg;
        _entityVisuals?.FlashSprite(_properties.HitColor, .1f, 2, true);
        _hpBar?.UpdateBar(this);

        if (Dead) OnDeath(dmg);
    }

    protected void KnockBack(Vector2 dir, float intensity)
    {
        SetRigidBodyVelocity((dir * intensity) * _properties.WeightFactor);
    }

    protected Vector2 GetRigidBodyVelocity() => _rb.velocity;

    protected void SetRigidBodyVelocity(float x = 0, float y = 0)
    {
        SetRigidBodyVelocity(new Vector2(x, y));
    }

    protected void SetRigidBodyVelocity(Vector2 newVelocity)
    {
        _rb.velocity = newVelocity;
    }
    protected void SetRigidBodyGravityScale(float newGravity)
    {
        _rb.gravityScale = newGravity;
    }




}
