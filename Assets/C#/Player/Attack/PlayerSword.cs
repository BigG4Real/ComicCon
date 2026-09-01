using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSword : MonoBehaviour
{
    [Header("HitBox")]
    [SerializeField] Vector2 attackHitBoxSize;
    [SerializeField] Vector2 attackHitBoxOffset;
    int normalAttackID;

    [Header("Pogo")]
    [SerializeField] Vector2 attackHitBoxSizePogo;
    [SerializeField] Vector2 attackHitBoxOffsetPogo;
    [SerializeField] float pogoBoost;
    bool pogo = false;
    int pogoAttackID;

    [Header("Up slash")]
    [SerializeField] Vector2 attackHitBoxOffsetUpSlash;
    bool upAttack = false;
    int upAttackID;

    [Header("Player")]
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;
    [SerializeField] HealthScript attacker;

    [Header("Damge")]
    [SerializeField] float dmg;

    [Header("Small Attack")]
    [SerializeField] float hitboxAppearTime;
    [SerializeField] float smallAttackCooldown;

    [Header("Full Attack")]
    [SerializeField] float FullHitboxAppearTime;
    [SerializeField] float FullAttackCooldownTime;
    bool isAttackingFull;

    [Header("Effect")]
    [SerializeField] GameObject slash;
    [SerializeField] hitboxManger hitboxManger;

    void Start()
    {
        normalAttackID = hitboxManger.AddHitbox(attackHitBoxSize, attackHitBoxOffset);
        pogoAttackID = hitboxManger.AddHitbox(attackHitBoxSizePogo, attackHitBoxOffsetPogo);
        upAttackID = hitboxManger.AddHitbox(attackHitBoxSizePogo, attackHitBoxOffsetUpSlash);
    }

    void Update()
    {
        List<(int id, float dmgAmount)> values = new List<(int, float)> {
            (normalAttackID, dmg),
            (pogoAttackID, dmg),
            (upAttackID, dmg),

        };

        values.ForEach(v =>
        {
            if (!hitboxManger.hitboxes[v.id].Activated) { return; }
            hitboxManger.DealDamgeToAllColliders(
                v.id,
                v.dmgAmount,
                hitboxManger.GetAllColliders(v.id),
                HealthScript.TeamSystem.player,
                Hit,
                HitInvis
            );
        });
    }

    void EnableHitbox(int id, float appearTime, float cooldown)
    {
        StartCoroutine(hitboxManger.ActiveHitbox(id, appearTime, cooldown));
        MakleSlashEffect(id);
    }

    void HitInvis()
    {
        player.HowManyExtraJumps = 1;
        if (pogo)
        {
            player.rb.linearVelocityY = 0;
            player.GravityAmount = player.DefualtGravity;
            player.rb.AddForce(Vector2.up * pogoBoost, ForceMode2D.Impulse);
        }
    }
    void Hit()
    {
        essence.GainEssence();
        player.HowManyExtraJumps = 1;
        if (pogo)
        {
            player.rb.linearVelocityY = 0;
            player.GravityAmount = player.DefualtGravity;
            player.rb.AddForce(Vector2.up * pogoBoost, ForceMode2D.Impulse);
        }
    }

    void MakleSlashEffect(int id)
    {
        GameObject slashEffect = Instantiate(
            slash,
            new Vector2(
                transform.position.x + hitboxManger.hitboxes[id].Offset.x * player.lastMoveDir.x,
                transform.position.y + hitboxManger.hitboxes[id].Offset.y
            ),
            transform.rotation);
        LookAt(slashEffect, transform);
        Destroy(slashEffect, 1);


        //Snådd kod från https://discussions.unity.com/t/transform-lookat-target-in-2d/105326
        void LookAt(GameObject effect, Transform Target)
        {
            Vector2 direction = Target.position - effect.transform.position;
            effect.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }
    }

    void FullAttackHitbox()
    {
        if (!isAttackingFull) { return; }
        player.rb.linearVelocityX = 0;
        player.rb.linearVelocityY = 0;
        EnableHitbox(normalAttackID, FullHitboxAppearTime, FullAttackCooldownTime);
        Invoke(nameof(FullAttackHitbox), FullAttackCooldownTime + FullHitboxAppearTime);
    }

    #region Inputs
    void OnShortAttack()
    {
        bool alreadyAttack = IsAttacking(new List<int> { pogoAttackID, upAttackID, normalAttackID });
        if (alreadyAttack) { return; }

        if (pogo)
            SpeficAttack(pogoAttackID);
        else if (upAttack)
            SpeficAttack(upAttackID);
        else
            SpeficAttack(normalAttackID);

        void SpeficAttack(int id)
        {
            if (hitboxManger.hitboxes[id].Activated && !hitboxManger.IsAnyHitboxActive()) { return; }
            EnableHitbox(id, hitboxAppearTime, smallAttackCooldown);
        }

        bool IsAttacking(List<int> allIds)
        {
            for (int i = 0; i < allIds.Count; i++)
            {
                if (hitboxManger.hitboxes[allIds[i]].Activated) { return true; }
            }
            return false;
        }
    }

    void OnUpOrDown(InputValue inputValue)
    {
        pogo = false;
        upAttack = false;
        if (inputValue.Get<Vector2>().y < 0)
            pogo = true;


        if (inputValue.Get<Vector2>().y > 0)
            upAttack = true;
    }

    void OnFullAttack()
    {
        if (!player.isGrounded) { return; }
        player.rb.simulated = false;
        isAttackingFull = true;
        FullAttackHitbox();
    }

    void OnAttackRelese()
    {
        isAttackingFull = false;
        player.rb.simulated = true;
    }
    #endregion

}
