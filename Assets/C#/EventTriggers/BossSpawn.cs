using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] hitboxManger hitboxManger;
    int HitboxId;
    [SerializeField] Vector2 HitBoxSize;
    [SerializeField] PlayerCamera PlayerCamera;
    [SerializeField] Transform Boss;
    [SerializeField] Transform Middle;
    void Start()
    {
        HitboxId = hitboxManger.AddHitbox(HitBoxSize, Vector2.zero);
    }

    void Update()
    {
        if(hitboxManger.GetAllColliders(HitboxId).Length > 0)
        {
            StartBoss();
        }
    }

    void StartBoss()
    {
        PlayerCamera.focusPoints.Add(Middle);
        PlayerCamera.focusPoints.Add(Boss);
        enabled = false;
    }
}
