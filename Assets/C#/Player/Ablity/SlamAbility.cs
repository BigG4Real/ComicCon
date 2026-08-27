using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlamAbility : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;
    [SerializeField] float downSpeed;
    bool DashDown;

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

        if (player.isGrounded)
        {
            DashDown = false;
            dmgScale = (float)Math.Round(dmgScale);
            StartCoroutine(hitboxManger.ActiveHitbox(hitboxFullDmgID, HitboxApperTime, Cooldown));
            StartCoroutine(hitboxManger.ActiveHitbox(hitboxFallOffDmgID, HitboxApperTime, Cooldown));
            
            values.ForEach(v =>
            {
                hitboxManger.DealDamgeToAllColliders(
                    v.id,
                    v.dmgAmount,
                    hitboxManger.GetAllColliders(v.id),
                    HealthScript.TeamSystem.player,
                    DidDamge
                );
            });
        }
    }

    void DidDamge()
    {
        
    }

    void Start()
    {
        hitboxFullDmgID = hitboxManger.AddHitbox(hitBoxSize, offSet);
        hitboxFallOffDmgID = hitboxManger.AddHitbox(hitBoxSizeFallOff, offSetFallOff);
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
        if (allowSpecial && !hitboxManger.hitboxes[hitboxFullDmgID].OnCooldwon && !player.isGrounded && essence.UseAbility(0))
        {
            if (hitboxManger.hitboxes[hitboxFullDmgID].FoundedHealths.Count > 0) hitboxManger.RemoveAllHealth(hitboxFullDmgID);
            dmgScale = minDmgAmount;
            player.rb.linearVelocityY = -downSpeed;
            player.rb.AddForce(-transform.up * downSpeed, ForceMode2D.Impulse);
            DashDown = true;
        }
    }


}
