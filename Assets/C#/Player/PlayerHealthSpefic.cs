using System.Collections;
using UnityEngine;

public class PlayerHealthSpefic : MonoBehaviour
{
    [SerializeField] HealthScript Health;
    float LastHealth;
    [SerializeField] float Time;

    void Start()
    {
        LastHealth = Health.Health;
    }

    void Update()
    {
        if (Health.Health != LastHealth)
        {
            LastHealth = Health.Health;
            StartCoroutine(ImunityFrames(Time));
        }
    }

    IEnumerator ImunityFrames(float time)
    {
        Health.canTakeDamge = false;
        yield return new WaitForSeconds(time);
        Health.canTakeDamge = true;
    }
}
