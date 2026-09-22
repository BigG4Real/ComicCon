using UnityEngine;

public class FinnishWithGame : MonoBehaviour
{
    [SerializeField] hitboxManger hitboxManger;
    int HitboxId;
    [SerializeField] Vector2 HitBoxSize;
    [SerializeField] Timer Timer;

    void Start()
    {
        HitboxId = hitboxManger.AddHitbox(HitBoxSize, Vector2.zero);
    }

    void Update()
    {
        if(hitboxManger.GetAllColliders(HitboxId).Length > 0)
        {
            Timer.LoadScoreboard();
            this.enabled = false;
        }
    }
}
