using System;
using System.Collections.Generic;
using UnityEngine;

public class LaserAbility : MonoBehaviour
{
    
    [Header("Player")]
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;
    [SerializeField] HealthScript attacker;

    [Header("Laser Hitbox")]
    [SerializeField] Vector2 hitBox;
    [SerializeField] Vector2 offSet;
    [SerializeField] float dmgAmount;
    [SerializeField] List<HealthScript> foundedHealths;
    [SerializeField] float HitboxApperTime;
    [SerializeField] float Cooldown;
    float lastMoveDir;
    bool hitboxUpp;
    bool canFire = true;
    void OnLaser()
    {
        if (!hitboxUpp && canFire && essence.UseAbility())
        {
            foundedHealths.Clear();
            hitboxUpp = true;
            canFire = false;
            Invoke(nameof(ResetFire), Cooldown);
            Invoke(nameof(ResetHitbox), HitboxApperTime);
            lastMoveDir = player.lastMoveDir.x;
        }
    }

    void Update()
    {
        if (hitboxUpp)
        {
            Fire();
        }
    }

    void Fire()
    {
        player.rb.linearVelocityY = 0;
        player.rb.linearVelocityX = 0;
                player.GravityAmount = player.DefualtGravity;
                
        Collider2D[] colliders;

        Vector2 hitboxPos = new Vector2(
            transform.position.x + offSet.x * transform.localScale.x * lastMoveDir,
            transform.position.y + offSet.y * transform.localScale.y
        );
        colliders = Physics2D.OverlapBoxAll(hitboxPos, hitBox, 0);

            for (int i = 0; i < colliders.Length; i++)
        {
            HealthScript health = colliders[i].GetComponentInChildren<HealthScript>();
            if (health == null) { continue; }
            bool dupicate = false;
            for (int j = 0; j < foundedHealths.Count; j++)
            {
                if (foundedHealths[j] == health)
                {
                    dupicate = true;
                    continue;
                }
            }
            if (dupicate || health.IsSameTeam(attacker.team)) { continue; }
            health.Dmg(dmgAmount, attacker.team);
            foundedHealths.Add(health);

            
        }
    }

    void ResetFire()
    {
        canFire = true;
    }
    void ResetHitbox()
    {
        hitboxUpp = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 hitboxPos = new Vector2(
            transform.position.x + offSet.x * transform.localScale.x* lastMoveDir,
            transform.position.y + offSet.y * transform.localScale.y
        );
        if (hitboxUpp)
        {
            Gizmos.DrawCube(hitboxPos, hitBox * transform.localScale);
        }
    }
}
