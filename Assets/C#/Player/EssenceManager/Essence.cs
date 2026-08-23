using System;
using UnityEngine;
using UnityEngine.UI;

public class Essence : MonoBehaviour
{
    [SerializeField] float essenceAmount;
    [SerializeField] Slider essenceSlider;
    [SerializeField] float sliderFillAmount;
    float sliderAmount;

    void Update()
    {
        essenceAmount = Math.Clamp(essenceAmount, 0, 100);
        if (sliderAmount < essenceAmount)
        {
            sliderAmount += sliderFillAmount * Time.deltaTime;
            sliderAmount = Math.Clamp(sliderAmount, 0, essenceAmount);
        }
        else if (sliderAmount > essenceAmount)
        {
            sliderAmount -= sliderFillAmount * Time.deltaTime;
            sliderAmount = Math.Clamp(sliderAmount, essenceAmount, 100);
        }
        essenceSlider.value = sliderAmount;
    }
    public void GainEssence(float amount = 25f / 4f)
    {
        essenceAmount += amount;
    }

    public bool UseAbility(float amount = 25)
    {
        bool CanUse = false;
        if (essenceAmount >= amount)
        {
            CanUse = true;
            essenceAmount -= amount;
        }
        return CanUse;
    }
}
