using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailFade : MonoBehaviour
{
    [SerializeField] private bool _fadeIn = false;
    [SerializeField] private bool _fadeOut = true;
    private void Awake() 
    {
        Color transparent = Color.black;
        transparent.a = 0;
        TrailRenderer tr = GetComponent<TrailRenderer>();

        if (_fadeIn)
            tr.startColor = transparent;
        if (_fadeOut)
            tr.endColor = transparent;
    }
}
