using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class RocksFromAbove : MonoBehaviour
{
    [SerializeField] hitboxManger hitboxManger;
    int hitBoxId;
    void Start()
    {
        float randomSize = Random.Range(-1f, 1f);
        transform.localScale = new Vector3(
            transform.localScale.x - randomSize,
            transform.localScale.y - randomSize,
            transform.localScale.z - randomSize
            );
        hitBoxId = hitboxManger.AddHitbox(new Vector2(1,1), Vector2.zero);
        StartCoroutine(hitboxManger.ActiveHitbox(hitBoxId, 999f, 0));
        Destroy(this.gameObject, 5);
    }

    void Update()
    {
        hitboxManger.DealDamgeToAllColliders(hitBoxId, 1, hitboxManger.GetAllColliders(hitBoxId), HealthScript.TeamSystem.eneamy);
    }

    void OnDestroy()
    {
        
    }
}
