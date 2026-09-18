using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ExplodeEneamy : HealthScript
{
    [SerializeField] List<MonoBehaviour> scripts = new List<MonoBehaviour>();
    [SerializeField] Collider2D eneamyCollider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator ani;
    [SerializeField] float knockbackAmount;
    [SerializeField] float knockbackAmountExtraUp;

    [SerializeField] List<Collider2D> eneamyPartsCollider = new List<Collider2D>();
    [SerializeField] List<Rigidbody2D> eneamyPartsRb = new List<Rigidbody2D>();
    public override void Death()
    {
        for (int i = 0; i < scripts.Count; i++)
        {
            scripts[i].enabled = false;
        }
        eneamyCollider.enabled = false;
        rb.simulated = false;
        ani.enabled = false;
        SetPartSate(true);
    }

    void Start()
    {
        SetPartSate(false);
    }

    void SetPartSate(bool state)
    {
        for (int i = 0; i < eneamyPartsCollider.Count; i++)
        {
            eneamyPartsCollider[i].enabled = state;
            eneamyPartsRb[i].simulated = state;
            if(state == true)
            {
                eneamyPartsRb[i].transform.parent = null;
                Vector2 DirToBomb = GameObject.FindWithTag("Player").transform.position - transform.position;
                eneamyPartsRb[i].AddForce(DirToBomb.normalized * knockbackAmount * -1 + Vector2.up *knockbackAmountExtraUp, ForceMode2D.Impulse);  
            }
        }
    }
}
