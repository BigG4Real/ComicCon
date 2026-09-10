using UnityEngine;

public class enviormentDmg : MonoBehaviour
{

    [Header("Attacks")]
    [SerializeField] hitboxManger hitbox;
    [SerializeField] Vector2 Size;
    int hitboxID;
    void Start()
    {
        hitboxID = hitbox.AddHitbox(Size, new Vector2());
    }

    void Update()
    {
        hitbox.DealDamgeToAllColliders(hitboxID, 1, hitbox.GetAllColliders(hitboxID), HealthScript.TeamSystem.eneamy);
        hitbox.RemoveAllHealth(hitboxID);
        StartCoroutine(hitbox.ActiveHitbox(hitboxID, 999, 0));
    }
}
