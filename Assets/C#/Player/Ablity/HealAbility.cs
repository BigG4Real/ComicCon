using System;
using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] HealthScript health;
    [SerializeField] float healAmount;
    [SerializeField] float healCooldown;
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;
    bool isHealing;
    [SerializeField] float essenceEatingSpeed;
    [SerializeField] float essenceToHeal;
    float essenceConsumed;

    void Update()
    {
        if(isHealing && player.isGrounded){ StartHeal(); }
    }

    void StartHeal()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        float eating = Math.Clamp(essenceEatingSpeed * Time.deltaTime, 0, essence.essenceAmount);
        essenceConsumed += eating;
        if(essenceConsumed > essenceToHeal)
        {
            essence.GainEssence(essenceConsumed - essenceToHeal);
        }
        if (essence.essenceAmount != 0 &&essence.UseAbility(eating) && essenceConsumed >= essenceToHeal)
        {
            Heal();
        }
    }

    void Heal()
    {
        essenceConsumed = 0;
        health.Heal(healAmount);
        isHealing = false;
        Invoke(nameof(OnHeal), healCooldown);
    }

    void InteruptHeal()
    {
        player.rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        essenceConsumed = 0;
    }


    void OnHeal()
    {
        isHealing = !isHealing;
        if(!isHealing){ InteruptHeal(); }
    }
}