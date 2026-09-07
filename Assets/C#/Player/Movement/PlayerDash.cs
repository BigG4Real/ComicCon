using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] MovementController player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float DashAmount;
    [SerializeField] float Time;
    [SerializeField] float ResetTime;
    public bool isDashing { get; private set; }
    bool CanDash = true;
    [SerializeField] Animator ani;

    void Update()
    {
        ani.SetBool("Dash", isDashing);
    }

    void ResetDash()
    {
        CanDash = true;
    }

    void OnDash()
    {
        if(!CanDash){ return; }
        isDashing = true;
        if (isDashing)
        {
            CanDash = false;
            rb.linearVelocityX = DashAmount * player.lastMoveDir.x;
            Invoke(nameof(StopDash), Time);
            Invoke(nameof(ResetDash), ResetTime);
        }
    }

    void StopDash()
    {
        isDashing = false;
        rb.linearVelocityX = player.Speed;
    }
}
