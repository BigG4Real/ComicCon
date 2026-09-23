using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserAbility : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] MovementController player;
    [SerializeField] Essence essence;

    [Header("Laser Hitbox")]
    [SerializeField] Vector2 hitBoxSize;
    [SerializeField] Vector2 offSet;
    [SerializeField] float dmgAmount;
    [SerializeField] float HitboxApperTime;
    [SerializeField] float Cooldown;
    [SerializeField] GameObject LaserVisule;
    [Header("Hitbox manager")]
    [SerializeField] hitboxManger hitboxManger;
    int hitboxID;

    void Start()
    {
        hitboxID = hitboxManger.AddHitbox(hitBoxSize, offSet);
    }

    bool allowSpecial = false;
    void OnUpOrDown(InputValue inputValue)
    {
        allowSpecial = false;
        if (Math.Round(inputValue.Get<Vector2>().y) == 0)
            allowSpecial = true;
    }

    void OnSpecial()
    {
        if (!hitboxManger.hitboxes[hitboxID].OnCooldwon && allowSpecial && !hitboxManger.IsAnyHitboxActive() && essence.UseAbility())
        {
            if(hitboxManger.hitboxes[hitboxID].FoundedHealths.Count > 0) hitboxManger.RemoveAllHealth(hitboxID);
            
            StartCoroutine(hitboxManger.ActiveHitbox(hitboxID, HitboxApperTime, Cooldown));
            
            GameObject laser = Instantiate(LaserVisule);
            int flip = hitboxManger.lookingDir.flipX ? -1 : 1;
            laser.transform.position = new Vector2(transform.position.x + (offSet.x * flip), transform.position.y + (offSet.y * flip));
            Destroy(laser, HitboxApperTime);
        }
    }

    void Update()
    {
        if (hitboxManger.hitboxes[hitboxID].Activated)
        {
            hitboxManger.DealDamgeToAllColliders(hitboxID, dmgAmount, hitboxManger.GetAllColliders(hitboxID), HealthScript.TeamSystem.player);
            player.rb.linearVelocityX = 0;
            player.rb.linearVelocityY = 0;
        }
    }
}
