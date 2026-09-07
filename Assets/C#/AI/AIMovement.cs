using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] float CheckDis;
    [SerializeField] LayerMask layer;

    [System.Serializable]
    public enum EneamySate
    {
        idle,
        walk,
        attacking,
        dead
    }
    [SerializeField] EneamySate sate;

    void Update()
    {
    }

    Vector2 CheckAllDiraction()
    {
        List<Vector2> checkDirs = new List<Vector2> { new Vector2(1, 0), new Vector2(2, 0) };

        for (int i = 0; i < checkDirs.Count; i++)
        {
            if (IsThereWall(checkDirs[i])) { return checkDirs[i]; };
        }
        return new Vector2(0,0);
    }

    bool IsThereWall(Vector2 diraction)
    {
        RaycastHit2D hit;
        return hit = Physics2D.Raycast(transform.position, diraction, CheckDis, layer);
    }
}
