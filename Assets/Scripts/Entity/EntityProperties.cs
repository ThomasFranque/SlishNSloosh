using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Entity Properties/Entity")]
public class EntityProperties : ScriptableObject
{
    [SerializeField] private GameObject _hpBarPrefab = null;
    [SerializeField] private float _totalHP = 10.0f;
    [SerializeField] private Color _hitColor = Color.red;
    [Range(0,2)] [SerializeField] private float _weightFactor = 1.0f;

    public float TotalHP => _totalHP;
    public Color HitColor => _hitColor;
    public float WeightFactor => _weightFactor;

    public HPBar NewHealthBar(Transform selfTransform)
    {
        HPBar newBar = Instantiate(_hpBarPrefab).GetComponent<HPBar>();        
        Vector3 barOffset = newBar.transform.position;
        newBar.transform.position = selfTransform.position + barOffset;
        newBar.transform.parent = selfTransform.parent;
        newBar.gameObject.SendMessage("UpdateBarValues", selfTransform);
        return newBar;
    }

}
