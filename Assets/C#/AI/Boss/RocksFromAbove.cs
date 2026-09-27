using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class RocksFromAbove : MonoBehaviour
{
    [SerializeField] hitboxManger hitboxManger;
    [SerializeField] float rotateSpeed;
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
        rotateSpeed = (Random.Range(-1, 1) == -1) ? -rotateSpeed : rotateSpeed;
        Destroy(this.gameObject, 5);
    }

    void Update()
    {
        hitboxManger.DealDamgeToAllColliders(hitBoxId, 1, hitboxManger.GetAllColliders(hitBoxId), HealthScript.TeamSystem.eneamy);
        transform.Rotate(new Vector3(0, 0, transform.rotation.z + rotateSpeed) * Time.deltaTime);
    }

    void OnDestroy()
    {
        
    }
}
