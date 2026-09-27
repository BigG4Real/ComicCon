using UnityEngine;

public class enviormentDmg : MonoBehaviour
{

    [Header("Attacks")]
    [SerializeField] hitboxManger hitbox;
    [SerializeField] Vector2 Size;
    int hitboxID;
    int hitboxIDEneamy;
    void Start()
    {
        hitboxID = hitbox.AddHitbox(Size, new Vector2());
        hitboxIDEneamy = hitbox.AddHitbox(Size, new Vector2());
    }

    void Update()
    {
        hitbox.DealDamgeToAllColliders(hitboxID, 1, hitbox.GetAllColliders(hitboxID), HealthScript.TeamSystem.eneamy);
        hitbox.DealDamgeToAllColliders(hitboxIDEneamy, 9999, hitbox.GetAllColliders(hitboxIDEneamy), HealthScript.TeamSystem.player);
        hitbox.RemoveAllHealth(hitboxID);
        hitbox.RemoveAllHealth(hitboxIDEneamy);
        StartCoroutine(hitbox.ActiveHitbox(hitboxID, 999, 0));
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDEneamy, 999, 0));
    }
}
