using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private GameObject _hitText = null;

    protected override void OnHit(float dmg, Vector2 hitDirection, float knockBackIntensity)
    {
        Instantiate(_hitText, transform.position, Quaternion.identity);
        KnockBack(hitDirection, knockBackIntensity);
        DealDamage(dmg);
    }
}
