using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private GameObject _hitText = null;

    protected override void OnHit(float dmg)
    {
        Instantiate(_hitText, transform.position, Quaternion.identity);
        DealDamage(dmg);
    }

    
}
