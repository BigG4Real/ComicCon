using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class FlyingEneamy : MonoBehaviour
{
    [SerializeField] AIMovement ai;

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
        hitbox.DealDamgeToAllColliders(hitboxID, 1, hitbox.GetAllColliders(hitboxID), HealthScript.TeamSystem.eneamy);
        hitbox.RemoveAllHealth(hitboxID);
        StartCoroutine(hitbox.ActiveHitbox(hitboxID, 999, 0));
    }
}
