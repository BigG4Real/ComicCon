using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class BasicEneamy : MonoBehaviour
{
    [SerializeField] AIMovement ai;

    [Header("Behavuir")]
    [SerializeField] float NeedToFollow;
    [SerializeField] Vector2 RandomStandTime;
    [SerializeField] Vector2 BetweenRandomStand;
    float BeforeStand;
    
    [Header("Attacks")]
    [SerializeField] hitboxManger hitbox;
    [SerializeField] Vector2 SizeOfEneamy;
    int hitboxIDCollider;
    [Header("Above Attack")]
    [SerializeField] AILos IsAbove;
    [SerializeField] Vector2 AttackAboveSize;
    [SerializeField] Vector2 AttackAboveOfset;
    int hitboxIDAbove;
    [Header("Forward Attack")]
    [SerializeField] AILos IsForward;
    [SerializeField] Vector2 AttackForwardSize;
    [SerializeField] Vector2 AttackForwardOfset;
    int hitboxIDForward;

    [Header("Animation")]
    [SerializeField] Animator ani;


    void Start()
    {
        hitboxIDCollider = hitbox.AddHitbox(SizeOfEneamy, new Vector2(), 6, false);
        hitboxIDAbove = hitbox.AddHitbox(AttackAboveSize, AttackAboveOfset, 6, false);
        hitboxIDForward = hitbox.AddHitbox(AttackForwardSize, AttackForwardOfset, 6, false);
        RemoveAllHealth();
    }

    void Update()
    {
        BeforeStand -= Time.deltaTime;
        if (BeforeStand <= 0)
        {
            BeforeStand = Random.Range((float)BetweenRandomStand.x, (float)BetweenRandomStand.y);
            if (NeedToFollow >= ai.DiractionTo(ai.FindNearest()).magnitude)
            {
                StartCoroutine(Stand(Random.Range((float)RandomStandTime.x, (float)RandomStandTime.y)));
            }
        }

        ani.SetBool("UpAttack", (IsAbove.Seeing.Count > 0));
        if(IsAbove.Seeing.Count > 0)
        {
            Invoke(nameof(AttackUp), 0.5f);
        }
        if(IsForward.Seeing.Count > 0)
        {
            Invoke(nameof(AttackForward), 0.5f);
        }

        hitbox.DealDamgeToAllColliders(hitboxIDCollider, 1, hitbox.GetAllColliders(hitboxIDCollider), HealthScript.TeamSystem.eneamy);
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDCollider, 999, 0));
    }

    void AttackUp()
    {
        if(!(IsAbove.Seeing.Count > 0)) return;
        hitbox.DealDamgeToAllColliders(hitboxIDAbove, 1, hitbox.GetAllColliders(hitboxIDAbove), HealthScript.TeamSystem.eneamy);
        hitbox.RemoveAllHealth(hitboxIDAbove);
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDAbove, 1f, 0));
    }

    void AttackForward()
    {
        ani.SetBool("ForwardAttack", (IsForward.Seeing.Count > 0));
        if(!(IsForward.Seeing.Count > 0)) return;
        hitbox.DealDamgeToAllColliders(hitboxIDForward, 1, hitbox.GetAllColliders(hitboxIDForward), HealthScript.TeamSystem.eneamy);
        hitbox.RemoveAllHealth(hitboxIDForward);
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDForward, 1f, 0));
    }

    void RemoveAllHealth()
    {
        hitbox.RemoveAllHealth(hitboxIDCollider);
        hitbox.RemoveAllHealth(hitboxIDAbove);
        Invoke(nameof(RemoveAllHealth), 0.3f);
    }

    IEnumerator Stand(float time)
    {
        ani.SetBool("Walk", false);
        ai.AllowMoveTo(false);
        yield return new WaitForSeconds(time);
        ani.SetBool("Walk", true);
        ai.AllowMoveTo(true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, NeedToFollow);
    }
}
