using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] hitboxManger hitboxManger;
    int HitboxId;
    [SerializeField] Vector2 HitBoxSize;
    [SerializeField] PlayerCamera PlayerCamera;
    [SerializeField] Transform Boss;
    [SerializeField] Transform Middle;
    [SerializeField] Animator ani;
    [SerializeField] HealthScript bossHealth;
    [SerializeField] BossAI ai;
    bool inFight;
    void Start()
    {
        HitboxId = hitboxManger.AddHitbox(HitBoxSize, Vector2.zero);
    }

    void Update()
    {
        if(hitboxManger.GetAllColliders(HitboxId).Length > 0 && !inFight)
        {
            StartBoss();
        }
        if(bossHealth.Health <= 0 && inFight)
        {
            EndBossFight();
        }
        if (ai.isDigging && PlayerCamera.focusPoints.Contains(Boss))
        {
            PlayerCamera.focusPoints.Remove(Boss); 
        }
        if (inFight && !ai.isDigging && !PlayerCamera.focusPoints.Contains(Boss))
        {
            PlayerCamera.focusPoints.Add(Boss);
        }
    }

    void EndBossFight()
    {
        PlayerCamera.focusPoints.Remove(Middle);
        PlayerCamera.focusPoints.Remove(Boss);
        ani.SetTrigger("OpenDoor");
    }

    void StartBoss()
    {
        inFight = true;
        PlayerCamera.focusPoints.Add(Middle);
        PlayerCamera.focusPoints.Add(Boss);
        Boss.gameObject.SetActive(true);
        ani.SetTrigger("CloseDoor");
    }
}
