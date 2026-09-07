using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Essence : MonoBehaviour
{
    [SerializeField] float essenceAmount;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Light2D glowLight;
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
        Color32 color = sprite.color;
        essenceSlider.value = sliderAmount;
        color.a = (byte)((float)(sliderAmount * 0.01f) * 255f); //https://stackoverflow.com/questions/37641472/how-do-i-calculate-a-percentage-of-a-number-in-c
        sprite.color = color;
        glowLight.intensity = (sliderAmount * 0.1f);
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
