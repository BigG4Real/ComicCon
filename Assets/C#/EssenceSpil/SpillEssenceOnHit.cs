using UnityEngine;

public class SpillEssenceOnHit : MonoBehaviour
{
    [SerializeField] GameObject essenceSpillObject;
    [SerializeField] HealthScript health;
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
        Instantiate(essenceSpillObject, transform.position, transform.rotation, transform);
    }
}
