using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthSpefic : HealthScript
{
    [SerializeField] float TimeWithImunity;
    float timer;

    public override void TookDmg()
    {
        timer = TimeWithImunity;
    }

    public override void Death()
    {
        SceneManager.LoadScene("Start");
    }

    void Update()
    {
        timer -= Time.deltaTime;
        canTakeDamge = timer <= 0 ? true : false;
    }
}
