using UnityEngine;

public class FinnishWithGame : MonoBehaviour
{
    [SerializeField] hitboxManger hitboxManger;
    int HitboxId;
    [SerializeField] Vector2 HitBoxSize;
    [SerializeField] Timer Timer;
    [SerializeField] Collider2D PlayerCollider;

    void Start()
    {
        HitboxId = hitboxManger.AddHitbox(HitBoxSize, Vector2.zero);
    }

    void Update()
    {
        Collider2D[] col = hitboxManger.GetAllColliders(HitboxId);
        for (int i = 0; i < col.Length; i++)
        {
            if(col[i] == PlayerCollider)
            {
                Timer.LoadScoreboard();
                this.enabled = false;
            }
        }
    }
}
