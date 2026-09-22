using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    
    [SerializeField] AIMovement ai;
    
    [Header("DMG collider")]
    [SerializeField] hitboxManger hitbox;
    [SerializeField] Vector2 SizeOfEneamy;
    int hitboxIDCollider;
    [Header("Above Attack")]

    [Header("Animation")]
    [SerializeField] Animator ani;


    void Start()
    {
        hitboxIDCollider = hitbox.AddHitbox(SizeOfEneamy, new Vector2(), 6, false);
        RemoveAllHealth();
    }

    void Update()
    {
        hitbox.DealDamgeToAllColliders(hitboxIDCollider, 1, hitbox.GetAllColliders(hitboxIDCollider), HealthScript.TeamSystem.eneamy);
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDCollider, 999, 0));
    }

    void RemoveAllHealth()
    {
        hitbox.RemoveAllHealth(hitboxIDCollider);
        Invoke(nameof(RemoveAllHealth), 0.3f);
    }

}
