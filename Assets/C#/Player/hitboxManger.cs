using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEngine.Rendering;
using System;

public class hitboxManger : MonoBehaviour
{
    [System.Serializable]
    public class Hitbox
    {
        [Header("Hitbox")]
        public Vector2 Size;
        public Vector2 Offset;
        [Header("Layer")]
        public LayerMask AvilibleLayers;
        [Header("Active")]
        public bool Activated;
        public bool OnCooldwon;
        public List<HealthScript> FoundedHealths = new List<HealthScript>();
    }
    public List<Hitbox> hitboxes;
    MovementController movement;

    void Start()
    {
        movement = GetComponent<MovementController>();
    }

    public void RemoveAllHealth(int index) => hitboxes[index].FoundedHealths.Clear();

    public int AddHitbox(Vector2 size, Vector2 offset, int avilibleLayersID = ~0)
    {
        Hitbox newHitBox = new Hitbox();
        newHitBox.Size = size;
        newHitBox.Offset = offset;
        newHitBox.AvilibleLayers = avilibleLayersID;
        hitboxes.Add(newHitBox);
        return hitboxes.Count - 1;
    }

    public IEnumerator ActiveHitbox(int index, float hitBoxUppTime, float cooldown)
    {
        Hitbox tempHitbox = hitboxes[index];
        if (tempHitbox.Activated) { yield return 0; }
        tempHitbox.Activated = true;
        tempHitbox.OnCooldwon = true;
        hitboxes[index] = tempHitbox;

        yield return new WaitForSeconds(hitBoxUppTime);

        tempHitbox.Activated = false;
        hitboxes[index] = tempHitbox;
        RemoveAllHealth(index);

        yield return new WaitForSeconds(cooldown);

        tempHitbox.OnCooldwon = false;
        hitboxes[index] = tempHitbox;
    }

    public Collider2D[] GetAllColliders(int index)
    {
        Collider2D[] colliders;

        Vector2 hitboxPos = new Vector2(
            transform.position.x + hitboxes[index].Offset.x * transform.localScale.x * movement.lastMoveDir.x,
            transform.position.y + hitboxes[index].Offset.y * transform.localScale.y
        );
        colliders = Physics2D.OverlapBoxAll(hitboxPos, hitboxes[index].Size, 0);
        
        return colliders;
    }

    public void DealDamgeToAllColliders(int index, float damgeAmount, Collider2D[] colliders, HealthScript.TeamSystem team, Action onHit = null)
    {
        if (!hitboxes[index].Activated) { return; }
        bool didHitSomething = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            HealthScript health = colliders[i].GetComponentInChildren<HealthScript>();
            if (health == null) { continue; }
            bool dupicate = false;
            for (int j = 0; j < hitboxes[index].FoundedHealths.Count; j++)
            {
                if (hitboxes[index].FoundedHealths[j] == health)
                {
                    dupicate = true;
                    continue;
                }
            }
            if (dupicate || health.IsSameTeam(team)) { continue; }
            didHitSomething = true;
            health.Dmg(damgeAmount, team);
            hitboxes[index].FoundedHealths.Add(health);
        }
        if (onHit != null && didHitSomething) onHit();
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (movement == null) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < hitboxes.Count; i++)
        {
            Vector2 hitboxPos = new Vector2(
                transform.position.x + hitboxes[i].Offset.x * transform.localScale.x * movement.lastMoveDir.x,
                transform.position.y + hitboxes[i].Offset.y * transform.localScale.y
            );
            if (hitboxes[i].Activated)
            {
                Gizmos.DrawCube(hitboxPos, hitboxes[i].Size * transform.localScale);
            }
            else
            {
                Gizmos.DrawWireCube(hitboxPos, hitboxes[i].Size * transform.localScale);
            }
        }
    }
#endif
}
