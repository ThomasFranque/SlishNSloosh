using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private byte _spritePixels = 32;
    [SerializeField] private Vector2Int amountOfTiles = Vector2Int.zero;
    [SerializeField] private Sprite[] _sprites = null;

    private void Awake()
    {
        Spawn();
    }

    private void Spawn()
    {
        Vector3Int startPivot = Vector3Int.zero;
        startPivot.x = -(amountOfTiles.x / 2 * _spritePixels);
        startPivot.y = -(amountOfTiles.y / 2 * _spritePixels);

        Vector3Int pivot = startPivot;

        for (int y = 0; y < amountOfTiles.y; y++)
        {
            for (int x = 0; x < amountOfTiles.x; x++)
            {
                NewTile(pivot);
                pivot.x += _spritePixels;
            }
            pivot.x = startPivot.x;
            pivot.y += _spritePixels;
        }
    }

    private void NewTile(Vector3Int position)
    {
        SpriteRenderer newSr =
            new GameObject($"BG Tile").AddComponent<SpriteRenderer>();
        newSr.sprite = _sprites[Random.Range(0, _sprites.Length)];
        newSr.sortingOrder = -500;
        newSr.transform.position = position;
        newSr.transform.parent = transform;
    }
}
