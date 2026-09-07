using System.Collections.Generic;
using UnityEngine;

//Kopierade kod från fight sim
public class AILos : MonoBehaviour
{
    [SerializeField] float CheckRadius;
    [SerializeField] float Fov;
    [SerializeField] LayerMask BlockLayer;


    [Header("Seeing eneamy")]
    public static List<GameObject> targets = new List<GameObject>();
    public List<GameObject> Seeing;
    [SerializeField] float SeeingTimeAmount;
    List<float> SeeingTimer = new List<float>();

    void Update()
    {
        if (targets.Count == 0) { return; }
        for (int i = 0; i < SeeingTimer.Count; i++)
        {
            SeeingTimer[i] -= Time.deltaTime;
            if (SeeingTimer[i] <= 0)
            {
                Seeing.RemoveAt(i);
                SeeingTimer.RemoveAt(i);
            }
        }
        foreach (GameObject t in targets)
        {
            CheckFor(t);
        }
    }

    bool CheckFor(GameObject LookForObj)
    {
        float dis = Vector2.Distance(transform.position, LookForObj.transform.position);
        if (dis > CheckRadius)
        {
            return false;
        }

        Vector2 DirToEneamy = LookForObj.transform.position - transform.position;
        float Angel = Vector2.Angle(transform.right, DirToEneamy);
        if (Angel > Fov / 2)
        {
            Debug.DrawRay(transform.position, DirToEneamy, Color.red);
            return false;
        }
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position, DirToEneamy, CheckRadius, BlockLayer))
        {
            if (hit.transform.gameObject != LookForObj)
            {
                Debug.DrawRay(transform.position, DirToEneamy, Color.red);
                return false;
            }
        }
        Debug.DrawRay(transform.position, DirToEneamy, Color.green);
        bool HaveObj = false;

        for (int i = 0; i < Seeing.Count; i++)
        {
            if (Seeing[i] == LookForObj)
            {
                SeeingTimer[i] = SeeingTimeAmount;
                HaveObj = true;
                break;
            }
        }

        if (!HaveObj)
        {
            Seeing.Add(LookForObj);
            SeeingTimer.Add(SeeingTimeAmount);
        }

        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CheckRadius);

        float fov = Fov / 2;
        Vector3 leftDir = Quaternion.Euler(0f, 0f, -fov) * transform.right;
        Vector3 rightDir = Quaternion.Euler(0f, 0f, fov) * transform.right;

        Gizmos.DrawLine(transform.position, transform.position + leftDir * CheckRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * CheckRadius);
    }
}
