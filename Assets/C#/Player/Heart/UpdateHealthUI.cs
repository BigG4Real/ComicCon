using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthUI : MonoBehaviour
{
    [Header("Get UI and Values")]
    [SerializeField] HealthScript HealthScript;
    [SerializeField] Slider HealthSlider;
    [SerializeField] Image[] Hearts;
    float LastHealth;
    
    [Header("UI effects")]
    [SerializeField] float DrainSpeed;

    [SerializeField] float PulseSpeed;
    [SerializeField] float PulseSize;
    float pulseTimer;
    Vector2 orginalSize;

    void Start()
    {
        orginalSize = Hearts[Hearts.Length -1].transform.localScale;
    }

    void Update()
    {
        if (HealthScript.Health != LastHealth)
        {
            LastHealth = HealthScript.Health;
        }

        int CurrentHeart = (int)Math.Round(LastHealth -1);
        HealthUI(CurrentHeart);
        HeartPulse(CurrentHeart);
    }

    void HeartPulse(int CurrentHeart)
    {
        if(!(LastHealth != 0 && Hearts[CurrentHeart] != null)) {return;}
        
        pulseTimer += Time.deltaTime * PulseSpeed;
        
        if(pulseTimer < 1 - PulseSize){ PulseSpeed = -PulseSpeed;}
        if(pulseTimer >  1 +PulseSize){ PulseSpeed = -PulseSpeed;}
        pulseTimer = Math.Clamp(pulseTimer, orginalSize.x - PulseSize, orginalSize.y +PulseSize);

        Hearts[CurrentHeart].transform.localScale = new Vector2(pulseTimer, pulseTimer);
    }

    void HealthUI(int CurrentHeart)
    {
        HealthSlider.value = Math.Clamp(HealthSlider.value -= Time.deltaTime * DrainSpeed, LastHealth - 1, HealthScript.MaxHealth - 1);

        for (int i = 0; i < Hearts.Length; i++)
        {
            Hearts[i].enabled = CurrentHeart >= i ? true : false;
        }
        Hearts[CurrentHeart].enabled = (LastHealth != 0 && Hearts[CurrentHeart]) ? true : false;
        Hearts[CurrentHeart].transform.localScale =
            (Hearts[CurrentHeart] != null) 
            ? orginalSize 
            : Hearts[CurrentHeart].transform.localScale;

        /*if (LastHealth != 0 && Hearts[CurrentHeart] != null)
        {
            Hearts[CurrentHeart].enabled = true;
            Hearts[CurrentHeart].transform.localScale = orginalSize;
        }
        if (CurrentHeart == -1 || (LastHealth != Hearts.Length && Hearts[CurrentHeart++] != null))
        {
            Hearts[CurrentHeart++].enabled = false;
            Hearts[CurrentHeart].transform.localScale = orginalSize; 
        }*/
    }
}
