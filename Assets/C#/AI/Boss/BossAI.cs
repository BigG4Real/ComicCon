using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] AIMovement ai;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D Hitbox;
    
    [Header("DMG collider")]
    [SerializeField] hitboxManger hitbox;
    [SerializeField] Vector2 SizeOfEneamy;
    int hitboxIDCollider;
    [Header("Roll Attack")]
    [SerializeField] float TimeBeforeRoll;
    [SerializeField] float PushForward;
    [SerializeField] float RollTime;
    [SerializeField] List<TrailRenderer> trails;
    bool isRolling;
    [Header("Underground Attack")]
    [SerializeField] float TimeBeforeDig;
    [SerializeField] float DigTime;
    [SerializeField] Transform RespawnAfterDig;
    [SerializeField] Vector2 PositionRocksSpawn;
    [SerializeField] Vector2 SizeOfRocksSpawn;
    [SerializeField] Vector2 SpawnTimeForRock;
    [SerializeField] GameObject RockPrefab;
    bool didDig;
    public bool isDigging {get; private set;}

    
    [Header("Animation")]
    [SerializeField] Animator ani;
    [SerializeField] Vector2 RandomWaitTimeAttacks;


    void Start()
    {
        hitboxIDCollider = hitbox.AddHitbox(SizeOfEneamy, new Vector2(), 6, false);
        RemoveAllHealth();
        GenerateAttack();
    }

    void GenerateAttack()
    {
        float random = Random.Range(RandomWaitTimeAttacks.x, RandomWaitTimeAttacks.y);
        int randomAttack = Random.Range(1, 3);
        if(!didDig && randomAttack == 1)
        {
            StartCoroutine(DigActive(random));
            random += TimeBeforeDig + DigTime;
            didDig = true;
        }
        else
        {
            StartCoroutine(RollActive(random));
            random += RollTime + TimeBeforeRoll;
            didDig = false;
        }
        Invoke(nameof(GenerateAttack), random);
    }

    void Update()
    {
        if(!isDigging)
            hitbox.DealDamgeToAllColliders(hitboxIDCollider, 2, hitbox.GetAllColliders(hitboxIDCollider), HealthScript.TeamSystem.eneamy);
        
        StartCoroutine(hitbox.ActiveHitbox(hitboxIDCollider, 999, 0));

        if(isRolling)
            Roll();
    }

    void Roll()
    {
        int dir = (sprite.flipX == false) ? 1 : -1;
        rb.angularDamping = 0;
        rb.AddForce(Vector2.right * dir * PushForward);
    }

    IEnumerator RollActive(float wait)
    {
        yield return new WaitForSeconds(wait);
        ani.SetBool("Roll", true);
        ani.SetTrigger("StartRoll");
        ai.AllowMoveTo(false);
        yield return new WaitForSeconds(TimeBeforeRoll);
        SetState(true);
        rb.angularDamping = 0;
        yield return new WaitForSeconds(RollTime);
        SetState(false);
        rb.angularDamping = 0.05f;

        void SetState(bool state)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].enabled = state;
            }
            ani.SetBool("Roll", state);
            isRolling = state;
            ai.AllowMoveTo(!state);
        }
    }


    IEnumerator DigActive(float wait)
    {
        yield return new WaitForSeconds(wait);
        SetState(true);
        isDigging = false;
        yield return new WaitForSeconds(TimeBeforeDig);
        isDigging = true;
        RockSpawn();
        yield return new WaitForSeconds(DigTime);
        transform.position = RespawnAfterDig.position;
        SetState(false);

        void SetState(bool state)
        {
            rb.simulated = !state;
            Hitbox.enabled = !state;
            ani.SetBool("Dig", state);
            isDigging = state;
            ai.AllowMoveTo(!state);
        }
    }

    void RockSpawn()
    {
        GameObject rock = RockPrefab;
        Instantiate(rock);
        rock.transform.position = new Vector2(PositionRocksSpawn.x + (Random.Range(-SizeOfRocksSpawn.x, SizeOfRocksSpawn.x)/2), PositionRocksSpawn.y + (Random.Range(-SizeOfRocksSpawn.y, SizeOfRocksSpawn.y))/2);

        if (isDigging)
        {
            Invoke(nameof(RockSpawn), Random.Range(SpawnTimeForRock.x, SpawnTimeForRock.y));
        }
    }

    void RemoveAllHealth()
    {
        hitbox.RemoveAllHealth(hitboxIDCollider);
        Invoke(nameof(RemoveAllHealth), 0.3f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(PositionRocksSpawn, SizeOfRocksSpawn);
    }

}
