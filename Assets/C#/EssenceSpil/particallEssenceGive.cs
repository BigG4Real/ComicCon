using System.Collections.Generic;
using UnityEngine;

public class particallEssenceGive : MonoBehaviour
{
    ParticleSystem ps;
    Essence essence;

    [SerializeField] float amountOfEssence;
    [HideInInspector] public float moveForawrd;

    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        ps = GetComponent<ParticleSystem>();
        essence = player.transform.parent.GetComponentInChildren<Essence>();
        ps.trigger.AddCollider(player.GetComponent<Collider2D>());
        LookAt(player.transform);
        transform.position += transform.up * moveForawrd;
        ps.Play();
    }

    //Snådd kod från https://discussions.unity.com/t/transform-lookat-target-in-2d/105326
    void LookAt(Transform Target)
    {
        Vector2 direction = Target.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            essence.GainEssence(amountOfEssence);
            ParticleSystem.Particle p = enter[i];
            p.remainingLifetime = 0;
            enter[i] = p;
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
