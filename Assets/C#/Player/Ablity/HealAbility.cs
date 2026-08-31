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
        if(isHealing){ StartHeal(); }
    }

    void StartHeal()
    {
        player.rb.simulated = false;
        essenceConsumed += essenceEatingSpeed * Time.deltaTime;
        if (essence.UseAbility(essenceEatingSpeed * Time.deltaTime) && essenceConsumed >= essenceToHeal)
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
        player.rb.simulated = true;
        essenceConsumed = 0;
    }


    void OnHeal()
    {
        isHealing = !isHealing;
        if(!isHealing){ InteruptHeal(); }
    }
}