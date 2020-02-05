using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private Image _staminaImage = null;

    [SerializeField] Vector2 _minMaxColorTransitionDuration = Vector2.zero;
    [SerializeField] private Color _warningStaminaColor = Color.red;
    [SerializeField] private Color _refillStaminaColor = Color.yellow;
    private Color _initialStaminaColor;
    private bool _towardsWarningColor;
    private float _t = 0f;

    public bool RefillingStamina { get; set; }
    public bool Depleeting { get; set; }
    private void Awake()
    {
        _towardsWarningColor = true;
        _initialStaminaColor = _staminaImage.color;
    }

    public void UpdateStaminaFill(float currentPercentage)
    {
        _staminaImage.fillAmount = currentPercentage;
    }

    private void Update()
    {
        if (RefillingStamina || Depleeting)
            UpdateStaminaBarWarningColor();
        else
        {
            _staminaImage.color = Color.white;
            _t = 0;
        }


    }

    private void UpdateStaminaBarWarningColor()
    {
        bool warningColorReached = false;
        bool normalColorReached = false;
        float duration = 0;
        Color color = Color.white;


        if (RefillingStamina)
        {
            color = _towardsWarningColor ?
            Color.Lerp(_initialStaminaColor, _refillStaminaColor, _t) :
            Color.Lerp(_refillStaminaColor, _initialStaminaColor, _t);
            duration =
                Mathf.Clamp(_staminaImage.fillAmount / _minMaxColorTransitionDuration[1],
                    _minMaxColorTransitionDuration[0],
                    _minMaxColorTransitionDuration[1]);
        }
        else
        {
            color = _towardsWarningColor ?
            Color.Lerp(_initialStaminaColor, _warningStaminaColor, _t) :
            Color.Lerp(_warningStaminaColor, _initialStaminaColor, _t);
            duration =
                Mathf.Clamp(_staminaImage.fillAmount / _minMaxColorTransitionDuration[1],
                    _minMaxColorTransitionDuration[0],
                    _minMaxColorTransitionDuration[1]);
        }


        _t += Time.deltaTime / duration;

        _staminaImage.color = color;

        warningColorReached = _staminaImage.color == _warningStaminaColor || _staminaImage.color == _refillStaminaColor;
        normalColorReached = _staminaImage.color == _initialStaminaColor;


        if ((warningColorReached && _towardsWarningColor) || (normalColorReached && !_towardsWarningColor))
        {
            _towardsWarningColor = !warningColorReached;
            _t = 0;
        }
    }
}
