using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerSword : MonoBehaviour
{
    [Header("HitBox")]
    [SerializeField] Vector2 attackHitBoxSize;
    [SerializeField] Vector2 attackHitBoxOffset;
    [Header("Pogo")]
    [SerializeField] Vector2 attackHitBoxSizePogo;
    [SerializeField] Vector2 attackHitBoxOffsetPogo;
    Vector2 pogoUpOrDown;
    bool pogo = false;
    [SerializeField] float pogoBoost;
    [Header("Player")]
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;
    [SerializeField] HealthScript attacker;

    [Header("Damge")]
    [SerializeField] float dmg;

    [Header("Small Attack")]
    [SerializeField] float hitboxAppearTime;
    bool isAttacking;
    [SerializeField] float smallAttackCooldown;
    float smallAttackTimer;

    [Header("Full Attack")]
    [SerializeField] float hitboxAppearTimeFull;
    [SerializeField] float AttackCooldownTime;
    bool isAttackingFull;
    float attackTimer;
    [Header("Effect")]
    [SerializeField] GameObject slash;


    [SerializeField] List<HealthScript> foundedHealths;
    Vector2 hitboxPos;

    void AttackDmg()
    {

        Collider2D[] colliders;
        UpdateHitBox();
        if (pogo)
        {
            colliders = Physics2D.OverlapBoxAll(hitboxPos, attackHitBoxSizePogo, 0);
        }
        else
        {
            colliders = Physics2D.OverlapBoxAll(hitboxPos, attackHitBoxSize * transform.localScale, 0);
        }
        bool OnlyOnePogo = false;
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
            health.Dmg(dmg, attacker.team);
            foundedHealths.Add(health);
            essence.GainEssence();
            player.HowManyExtraJumps = 1;
            if (pogo && !OnlyOnePogo && (float)Math.Round(pogoUpOrDown.y) < 0)
            {
                OnlyOnePogo = false;
                player.rb.linearVelocityY = 0;
                player.GravityAmount = player.DefualtGravity;
                player.rb.AddForce(Vector2.up * pogoBoost, ForceMode2D.Impulse);

            }
        }
    }

    void UpdateHitBox()
    {
        if (pogo)
        {
            hitboxPos = new Vector2(
            transform.position.x + attackHitBoxOffsetPogo.x * transform.localScale.x,
            transform.position.y + attackHitBoxOffsetPogo.y* transform.localScale.y * (float)Math.Round(pogoUpOrDown.y)
            );
        }
        else
        {
            hitboxPos = new Vector2(
            transform.position.x + (attackHitBoxOffset.x * player.lastMoveDir.x) * transform.localScale.x,
            transform.position.y + attackHitBoxOffset.y * transform.localScale.y
            );
        }
    }

    void MakleSlashEffect()
    {
        UpdateHitBox();
        GameObject slashEffect = Instantiate(slash, hitboxPos, transform.rotation);
        LookAt(slashEffect, transform);
        Destroy(slashEffect, 2.5f);
    }

    //Snådd kod från https://discussions.unity.com/t/transform-lookat-target-in-2d/105326
    void LookAt(GameObject effect, Transform Target)
    {
        Vector2 direction = Target.position - effect.transform.position;
        effect.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    void Update()
    {
        smallAttackTimer -= Time.deltaTime;
        if (isAttackingFull)
        {
            player.rb.linearVelocityX = 0;
            player.rb.linearVelocityY = 0;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                AttackManyFull();
            }
        }
        if (isAttacking) { AttackDmg(); }
        else if(foundedHealths.Count > 0)
        {
            foundedHealths.Clear();
        }
    }

    void RemoveAttack()
    {
        isAttacking = false;
    }

    void AttackManyFull()
    {
        MakleSlashEffect();
        smallAttackTimer = smallAttackCooldown;
        attackTimer = AttackCooldownTime;
        isAttacking = true;
        Invoke(nameof(RemoveAttack), hitboxAppearTimeFull);
    }

    #region Inputs
    void OnShortAttack()
    {
        isAttackingFull = false;
        if (isAttacking || smallAttackTimer > 0) { return; }
        MakleSlashEffect();
        isAttacking = !isAttacking;
        smallAttackTimer = smallAttackCooldown;

        Invoke(nameof(RemoveAttack), hitboxAppearTime);
    }

    void OnUpOrDown(InputValue inputValue)
    {
        pogoUpOrDown = inputValue.Get<Vector2>();
        pogo = !pogo;
    }

    void OnFullAttack()
    {
        if(!player.isGrounded) { return; }
        player.rb.simulated = false;
        isAttackingFull = true;
        attackTimer = AttackCooldownTime;
    }

    void OnAttackRelese()
    {
        isAttacking = false;
        isAttackingFull = false;
        player.rb.simulated = true;
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 hitboxPos;
        if (pogo)
        {
            hitboxPos = new Vector2(
            transform.position.x + attackHitBoxOffsetPogo.x * transform.localScale.x,
            transform.position.y + attackHitBoxOffsetPogo.y * transform.localScale.y* (float)Math.Round(pogoUpOrDown.y)
            );
            if (isAttacking)
            {
                Gizmos.DrawCube(hitboxPos, attackHitBoxSizePogo * transform.localScale);
            }
            else
            {
                Gizmos.DrawWireCube(hitboxPos, attackHitBoxSizePogo * transform.localScale);
            }
        }
        else
        {
            hitboxPos = new Vector2(
            transform.position.x + (attackHitBoxOffset.x * player.lastMoveDir.x* transform.localScale.x),
            transform.position.y + attackHitBoxOffset.y* transform.localScale.y
            );
            if (isAttacking)
            {
                Gizmos.DrawCube(hitboxPos, attackHitBoxSize* transform.localScale);
            }
            else
            {
                Gizmos.DrawWireCube(hitboxPos, attackHitBoxSize* transform.localScale);
            }
        }

    }
}
