using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private Slider _manaSlider;
    public void SetMaxMana(int maxMana)
    {
        _manaSlider.maxValue = maxMana;
        _manaSlider.value = maxMana;
    }
    public void UpdateMana(int currentMana)
    {
        _manaSlider.value = currentMana;
    }
}
