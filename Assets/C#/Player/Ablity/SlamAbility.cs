using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlamAbility : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;
    [SerializeField] float upSpeed;
    [SerializeField] float timeBeforeFall;
    [SerializeField] float downSpeed;
    bool DashDown;
    bool PreformeSpecial;

    [Header("Slam Hitbox")]
    [SerializeField] Vector2 hitBoxSize;
    [SerializeField] Vector2 offSet;
    [SerializeField] float minDmgAmount;
    [SerializeField] float maxDmgAmount;
    [SerializeField] float dmgScaleAmount;
    float dmgScale;
    [SerializeField] float HitboxApperTime;
    [SerializeField] float Cooldown;

    [Header("Slam Hitbox Fall Off")]
    [SerializeField] Vector2 hitBoxSizeFallOff;
    [SerializeField] Vector2 offSetFallOff;
    [SerializeField] float dmgFallOffDivide;

    [Header("Hitbox manager")]
    [SerializeField] hitboxManger hitboxManger;
    int hitboxFullDmgID;
    int hitboxFallOffDmgID;


    void Update()
    {
        if (!DashDown) { return; }
        List<(int id, float dmgAmount)> values = new List<(int, float)> {
            (hitboxFullDmgID, dmgScale),
            (hitboxFallOffDmgID, dmgScale / dmgFallOffDivide),
        };
        dmgScale += dmgScale * dmgScaleAmount * Time.deltaTime;
        dmgScale = (float)Math.Clamp(dmgScale, minDmgAmount, maxDmgAmount);

        player.rb.linearVelocityY = -downSpeed;
        player.rb.AddForce(-transform.up * downSpeed, ForceMode2D.Force);

        if (player.isGrounded || CanHit())
        {
            hitboxManger.hitboxes[hitboxFullDmgID].Activated = false;
            hitboxManger.RemoveAllHealth(hitboxFullDmgID);

            PreformeSpecial = false;
            DashDown = false;
            dmgScale = (float)Math.Round(dmgScale);
            StartCoroutine(hitboxManger.ActiveHitbox(hitboxFullDmgID, HitboxApperTime, Cooldown));
            StartCoroutine(hitboxManger.ActiveHitbox(hitboxFallOffDmgID, HitboxApperTime, Cooldown));
            
            bool oneTime = false;
            values.ForEach(v =>
            {
                oneTime = true;
                hitboxManger.DealDamgeToAllColliders(
                    v.id,
                    v.dmgAmount,
                    hitboxManger.GetAllColliders(v.id),
                    HealthScript.TeamSystem.player
                );
            });
            if(oneTime) DidDamge();
        }
    }

    bool CanHit()
    {
        hitboxManger.hitboxes[hitboxFullDmgID].Activated = true;
        hitboxManger.RemoveAllHealth(hitboxFullDmgID);
        return hitboxManger.DealDamgeToAllColliders(
                    hitboxFullDmgID,
                    0,
                    hitboxManger.GetAllColliders(hitboxFullDmgID),
                    HealthScript.TeamSystem.player,
                    DidDamge
                );
    }

    void DidDamge()
    {
        player.rb.linearVelocityY = 0;
        player.rb.AddForce(Vector2.up * player.JumpForceAmount, ForceMode2D.Impulse);
        player.HowManyExtraJumps--;
    }

    void Start()
    {
        hitboxFullDmgID = hitboxManger.AddHitbox(hitBoxSize, offSet);
        hitboxFallOffDmgID = hitboxManger.AddHitbox(hitBoxSizeFallOff, offSetFallOff);
    }

    void DashDownEnable()
    {
        DashDown = true;
    }

    bool allowSpecial = false;
    void OnUpOrDown(InputValue inputValue)
    {
        allowSpecial = false;
        if (inputValue.Get<Vector2>().y < 0)
            allowSpecial = true;
    }

    void OnSpecial()
    {
        if (allowSpecial && !hitboxManger.hitboxes[hitboxFullDmgID].OnCooldwon && !PreformeSpecial&& essence.UseAbility())
        {
            if (hitboxManger.hitboxes[hitboxFullDmgID].FoundedHealths.Count > 0) hitboxManger.RemoveAllHealth(hitboxFullDmgID);
            PreformeSpecial = true;
            dmgScale = minDmgAmount;
            player.rb.linearVelocityY = upSpeed;
            player.rb.AddForce(transform.up * upSpeed, ForceMode2D.Impulse);
            Invoke(nameof(DashDownEnable), timeBeforeFall);
        }
    }


}
