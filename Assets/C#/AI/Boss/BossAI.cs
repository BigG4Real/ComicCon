using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] AIMovement ai;
    [SerializeField] Rigidbody2D rb;
    
    [Header("DMG collider")]
    [SerializeField] hitboxManger hitbox;
    [SerializeField] Vector2 SizeOfEneamy;
    int hitboxIDCollider;
    [Header("Roll Attack")]
    [SerializeField] float TimeBeforeRoll;
    [SerializeField] float PushForward;
    [SerializeField] float RollTime;
    bool isRolling;

    [Header("Animation")]
    [SerializeField] Animator ani;


    void Start()
    {
        hitboxIDCollider = hitbox.AddHitbox(SizeOfEneamy, new Vector2(), 6, false);
        RemoveAllHealth();
        
        StartCoroutine(RollActive());
    }

    void Update()
    {
        hitbox.DealDamgeToAllColliders(hitboxIDCollider, 1, hitbox.GetAllColliders(hitboxIDCollider), HealthScript.TeamSystem.eneamy);
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDCollider, 999, 0));

        if(isRolling)
            Roll();
    }

    void Roll()
    {
        int dir = (sprite.flipX == false) ? 1 : -1;
        rb.AddForce(Vector2.right * dir * PushForward);
    }

    IEnumerator RollActive()
    {
        ani.SetBool("Roll", true);
        ani.SetTrigger("StartRoll");
        ai.AllowMoveTo(false);
        yield return new WaitForSeconds(TimeBeforeRoll);
        SetState(true);
        yield return new WaitForSeconds(RollTime);
        SetState(false);
        void SetState(bool state)
        {
            ani.SetBool("Roll", state);
            isRolling = state;
            ai.AllowMoveTo(!state);
        }
    }



    void RemoveAllHealth()
    {
        hitbox.RemoveAllHealth(hitboxIDCollider);
        Invoke(nameof(RemoveAllHealth), 0.3f);
    }

}
