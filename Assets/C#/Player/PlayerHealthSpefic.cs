using System.Collections;
using UnityEngine;

public class PlayerHealthSpefic : MonoBehaviour
{
    [SerializeField] HealthScript Health;
    public float LastHealth;
    [SerializeField] float TimeWithImunity;
    float timer;

    void Start()
    {
        LastHealth = Health.Health;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (Health.Health < LastHealth)
        {
            timer = TimeWithImunity;
        }
        Health.canTakeDamge = timer <= 0 ? true : false;
        LastHealth = Health.Health;
    }
}
