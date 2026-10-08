using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class NuggetChokis : MonoBehaviour
{
    [SerializeField] AIMovement ai;

    [Header("Behavuir")]
    [SerializeField] float NeedToFollow;
    [SerializeField] Vector2 RandomStandTime;
    [SerializeField] Vector2 BetweenRandomStand;
    float BeforeStand;

    [Header("Animation")]
    [SerializeField] Animator ani;


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
