using System.Collections.Generic;
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
    bool canCahngeDiraction = true;

    [SerializeField] AILos aiLos;

    void Update()
    {
        SetPosition();
        Walk(CheckAllDiraction());
    }

    void GoToPlayer()
    {
    }

    void Walk(Vector2 checkDiraction)
    {
        if(sate != EneamySate.walk) { return; }
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
}
