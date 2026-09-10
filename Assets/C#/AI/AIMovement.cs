using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] float CheckDis;
    [SerializeField] LayerMask layer;

    [System.Serializable]
    public enum EneamySate
    {
        walk,
        attacking,
        dead
    }
    [SerializeField] EneamySate sate;
     Vector2 checkPosition;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHight;
    bool canCahngeDiraction = true;

    [SerializeField] AILos aiLos;

    bool canJump = true;
    [SerializeField] float jumpCooldown = 0.75f;

    [SerializeField] bool AllowMove = true;

    [SerializeField] SpriteRenderer sprite;

    void Update()
    {
        SetPosition();
        UpdateLos(FindNearest());
        if (aiLos.Seeing.Count > 0 && AllowMove)
        {
            GoTo(FindNearest());
            return;
        }
        if(AllowMove)
            Walk(CheckAllDiraction());
    }

    void UpdateLos(GameObject obj)
    {
        Vector2 LookTo = new Vector2(0,0);
        Quaternion newQuaternion = new Quaternion();
        if (obj != null)
        {
            LookTo = obj.transform.position - transform.position;
            LookTo = new Vector2(Math.Clamp(LookTo.x, -1, 1), 0);
        }
        if (LookTo == new Vector2(1, 0) || walkSpeed > 0)
        {
            sprite.flipX = false;
            newQuaternion.Set(0, 0, 0, 1);
            aiLos.transform.rotation = newQuaternion;
        }
        else
        {
            sprite.flipX = true;
            newQuaternion.Set(0, 180, 0, 1);
            aiLos.transform.rotation = newQuaternion;
        }
    }

    void GoTo(GameObject obj)
    {
        sate = EneamySate.attacking;
        Run(new Vector2(Math.Clamp(DiractionTo(obj).x, -1, 1), 0));
    }

    void Run(Vector2 walkDir)
    {
        rb.linearVelocityX = runSpeed * walkDir.x;
        if (CheckAllDiraction() != new Vector2(0, 0) && canJump)
        {
            canJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
            rb.AddForce(transform.up * jumpHight, ForceMode2D.Impulse);
        }
    }

    void ResetJump()
    {
        canJump = true;
    }

    void Walk(Vector2 checkDiraction)
    {
        sate = EneamySate.walk;
        rb.linearVelocityX = walkSpeed;
        if(checkDiraction != new Vector2(0,0) && canCahngeDiraction) 
        { 
            walkSpeed *= -1; 
            canCahngeDiraction = false; 
            Invoke(nameof(ResetDiraction), 0.5f); 
        }
    }

    void ResetDiraction()
    {
        canCahngeDiraction = true;
    }

    void SetPosition()
    {
        checkPosition = new Vector2(transform.position.x, transform.position.y);
        checkPosition.y -= 0.1f;
    }

    Vector2 CheckAllDiraction()
    {
        List<Vector2> checkDirs = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0) };

        for (int i = 0; i < checkDirs.Count; i++)
        {
            if (Physics2D.Raycast(checkPosition, checkDirs[i], CheckDis, layer)) 
            {
                Debug.DrawRay(checkPosition, checkDirs[i] * CheckDis, Color.green);
                return checkDirs[i]; 
            };
            Debug.DrawRay(checkPosition, checkDirs[i] * CheckDis, Color.blue);
        }
        return new Vector2(0,0);
    }

    
    public GameObject FindNearest()
    {
        float dis = Mathf.Infinity;
        GameObject closest = null;
        for (int i = 0; i < aiLos.Seeing.Count; i++)
        {
            float val = Vector2.Distance(aiLos.Seeing[i].transform.position, transform.position);
            if (val < dis)
            {
                closest = aiLos.Seeing[i];
            }
        }
        return closest;
    }

    public Vector2 DiractionTo(GameObject obj){
        if (obj == null) return new Vector2(0,0);
        Vector2 DirToEneamy = obj.transform.position - transform.position;
        return DirToEneamy;
    }

    public void AllowMoveTo(bool value){
        AllowMove = value;
        rb.linearVelocityX = 0;
    }
}
