using UnityEngine;
using UnityEngine.Rendering.Universal;

//Kopierade koden frpn Fight Sim och andrade den lite grann
public class lightFlicker : MonoBehaviour
{
    [SerializeField] Vector2 MinAndMax;
    float random;

    [SerializeField] float Speed;

    [SerializeField] float defult;
    [SerializeField] float current;
    [SerializeField] float target;
    [SerializeField] Light2D lights;

    [SerializeField] float LifeTime;
    [SerializeField] float KillSpeed;

    void Start()
    {
        defult = lights.intensity;
        current = Random.Range(MinAndMax.x + defult, MinAndMax.y + defult);
        target = Random.Range(MinAndMax.x + defult, MinAndMax.y + defult);
    }
    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime > 0)
        {
            Flicker();
            return;
        }
        KillLight();
    }

    void KillLight()
    {
        lights.intensity -= KillSpeed * Time.deltaTime;
        if (lights.intensity <= 0)
        {
            lights.intensity = 0;
        }
    }

    void Flicker()
    {
        current = Mathf.MoveTowards(current, target, Time.deltaTime * Speed);

        if (Mathf.Abs(current - target) < 0.05f)
        {
            target = Random.Range(MinAndMax.x + defult, MinAndMax.y + defult);
        }

        lights.intensity = current + defult;
    }
}
