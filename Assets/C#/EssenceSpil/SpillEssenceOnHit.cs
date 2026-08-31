using UnityEngine;

public class SpillEssenceOnHit : MonoBehaviour
{
    [SerializeField] GameObject essenceSpillObject;
    [SerializeField] HealthScript health;
    [SerializeField] float moveOut;
    float lastHealth;
    void Start()
    {
        lastHealth = health.Health;
    }
    void Update()
    {
        if (lastHealth != health.Health)
        {
            Destroy(SpillEssence(), 1.5f);
            lastHealth = health.Health;
        }
    }

    GameObject SpillEssence()
    {
        GameObject obj = Instantiate(essenceSpillObject, transform.position, transform.rotation, transform);
        obj.GetComponent<particallEssenceGive>().moveForawrd = moveOut;
        return obj;
    }
}
