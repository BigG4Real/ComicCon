using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] List<TrailRenderer> trails;
    [SerializeField] Vector2 RandomWaitTime;
    bool isRolling;

    [Header("Animation")]
    [SerializeField] Animator ani;


    void Start()
    {
        hitboxIDCollider = hitbox.AddHitbox(SizeOfEneamy, new Vector2(), 6, false);
        RemoveAllHealth();
        
        StartCoroutine(RollActive(Random.Range(RandomWaitTime.x, RandomWaitTime.y)));
    }

    void Update()
    {
        hitbox.DealDamgeToAllColliders(hitboxIDCollider, 2, hitbox.GetAllColliders(hitboxIDCollider), HealthScript.TeamSystem.eneamy);
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDCollider, 999, 0));

        if(isRolling)
            Roll();
    }

    void Roll()
    {
        int dir = (sprite.flipX == false) ? 1 : -1;
        rb.angularDamping = 0;
        rb.AddForce(Vector2.right * dir * PushForward);
    }

    IEnumerator RollActive(float wait)
    {
        yield return new WaitForSeconds(wait);
        ani.SetBool("Roll", true);
        ani.SetTrigger("StartRoll");
        ai.AllowMoveTo(false);
        yield return new WaitForSeconds(TimeBeforeRoll);
        SetState(true);
        rb.angularDamping = 0;
        yield return new WaitForSeconds(RollTime);
        SetState(false);
        rb.angularDamping = 0.05f;
        StartCoroutine(RollActive(Random.Range(RandomWaitTime.x, RandomWaitTime.y)));
        void SetState(bool state)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].enabled = state;
            }
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
