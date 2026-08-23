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
    public bool canTakeDamge = true;

    public virtual void Heal(float healAmount)
    {
        Health += healAmount;
    }
    public virtual void Dmg(float damgeAmount, TeamSystem attacker)
    {
        if (!canTakeDamge && !IsSameTeam(attacker))
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

    public bool IsSameTeam(TeamSystem teamCheck) {
        bool isSameTeam = false;
        if (team == teamCheck)
        {
            isSameTeam = true;
        }
        return isSameTeam;
    }
}
