using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Slider staminaSlider;

    private void OnEnable()
    {
        PlayerControl.OnStaminaChanged += UpdateStaminaBar;
    }

    private void OnDisable()
    {
        PlayerControl.OnStaminaChanged -= UpdateStaminaBar;
    }

    private void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }
}