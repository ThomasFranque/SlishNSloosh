using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    private const string _SHADOW_OBJ_NAME = " Drop Shadow";
    [SerializeField] private Vector2 _shadowOffset = new Vector2(2.0f, 0);
    [SerializeField] private Color _initialShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.150f);

    private SpriteRenderer _targetSpriteRenderer;
    private SpriteRenderer _shadowSpriteRenderer;

    private void Awake()
    {
        _targetSpriteRenderer = GetComponent<SpriteRenderer>();
        _shadowSpriteRenderer = SpawnDropShadow();

        UpdateDropShadow();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDropShadow();
    }

    private SpriteRenderer SpawnDropShadow()
    {
        SpriteRenderer dropShadowSR =
            new GameObject(_targetSpriteRenderer.name + _SHADOW_OBJ_NAME).AddComponent<SpriteRenderer>();
            
        dropShadowSR.sortingOrder = _targetSpriteRenderer.sortingOrder - 1;
        dropShadowSR.color = _initialShadowColor;

        dropShadowSR.transform.transform.position = _targetSpriteRenderer.transform.position;
        dropShadowSR.transform.parent = transform;

        return dropShadowSR;
    }

    private void UpdateDropShadow()
    {
        _shadowSpriteRenderer.transform.position = 
            _targetSpriteRenderer.transform.position + (Vector3)_shadowOffset;
        _shadowSpriteRenderer.sprite = _targetSpriteRenderer.sprite;
    }

    public void ChangeShadowColor(Color newColor)
    {
        _shadowSpriteRenderer.color = newColor;
    }
}
