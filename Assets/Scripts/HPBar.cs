using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private const float _DAMAGE_BAR_SPEED = 5.0f;

    [SerializeField] private SpriteMask _innerBarMask = null;
    [SerializeField] private SpriteMask _damageDisplayMask = null;

    private void Update() 
    {
        UpdateDamageBar();
    }

    public void UpdateBar(Entity e)
    {
        UpdateHPMask(e);
    }

    private void UpdateHPMask(Entity e) 
    {
        Vector3 newMaskScale = _innerBarMask.transform.localScale;
        newMaskScale.x = e.HP / e.TotalHP;
        _innerBarMask.transform.localScale = newMaskScale;

        // if (_damageDisplayMask.transform.localScale.x > _innerBarMask.transform.localScale.x)
        // {
        //     Vector3 newScale = _damageDisplayMask.transform.localScale;
        //     newScale.x = _innerBarMask.transform.localScale.x;
        //     _damageDisplayMask.transform.localScale = newScale;
        // }
    }

    private void UpdateDamageBar()
    {
        float newXScale = 
            Mathf.Lerp(
                _damageDisplayMask.transform.localScale.x, 
                _innerBarMask.transform.localScale.x- 0.06f, 
                Time.deltaTime * (_DAMAGE_BAR_SPEED * _damageDisplayMask.transform.localScale.x));
                
        Vector3 newScale = _damageDisplayMask.transform.localScale;
        newScale.x = newXScale;
        _damageDisplayMask.transform.localScale = newScale;
    }
}
