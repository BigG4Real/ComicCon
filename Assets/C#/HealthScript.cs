using System;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [System.Serializable]
    public enum TeamSystem {
        eneamy,
        player,
        obj
    }
    public TeamSystem team;

    public float Health;
    public float MaxHealth;
    public bool canTakeDamge = true;

    void Start()
    {
        if (MaxHealth == 0)
        {
            MaxHealth = Health;
        }
    }

    public virtual void Heal(float healAmount)
    {
        Health = Math.Clamp(Health + healAmount, 0, MaxHealth);
    }
    public virtual void Dmg(float damgeAmount, TeamSystem attacker)
    {
        if (canTakeDamge && !IsSameTeam(attacker))
        {
            Health -= damgeAmount;
        }
        if (Health <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public bool IsSameTeam(TeamSystem teamCheck) => team == teamCheck;
    
}
