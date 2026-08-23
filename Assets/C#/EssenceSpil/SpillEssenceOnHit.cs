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
            SpillEssence();
            lastHealth = health.Health;
        }
    }

    void SpillEssence()
    {
        GameObject obj = essenceSpillObject;
        Instantiate(obj, transform.position, transform.rotation, transform);
        obj.GetComponent<particallEssenceGive>().moveForawrd = moveOut;
    }
}
