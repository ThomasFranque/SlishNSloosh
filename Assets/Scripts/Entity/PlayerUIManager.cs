using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private Image _staminaImage = null;
    
    public void UpdateStaminaFill(float currentPercentage)
    {
        _staminaImage.fillAmount = currentPercentage;
    }
}
