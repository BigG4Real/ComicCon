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
    [SerializeField] Vector2 Size;
    int hitboxID;

    void Start()
    {
        hitboxID = hitbox.AddHitbox(Size, new Vector2(), 6, false);
    }

    void Update()
    {
        BeforeStand -= Time.deltaTime;
        GameObject nearestObj = null;
        if (BeforeStand <= 0)
        {
            BeforeStand = Random.Range((float)BetweenRandomStand.x, (float)BetweenRandomStand.y);
            if (NeedToFollow >= ai.DiractionTo(ai.FindNearest()).magnitude)
            {
                StartCoroutine(Stand(Random.Range((float)RandomStandTime.x, (float)RandomStandTime.y)));
            }
        }
        hitbox.DealDamgeToAllColliders(hitboxID, 1, hitbox.GetAllColliders(hitboxID), HealthScript.TeamSystem.eneamy);
        hitbox.RemoveAllHealth(hitboxID);
        StartCoroutine(hitbox.ActiveHitbox(hitboxID, 999, 0));
    }

    IEnumerator Stand(float time)
    {
        ai.AllowMoveTo(false);
        yield return new WaitForSeconds(time);
        ai.AllowMoveTo(true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, NeedToFollow);
    }
}
