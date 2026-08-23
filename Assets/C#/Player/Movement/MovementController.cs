using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [Header("Movement")]
    public Rigidbody2D rb;
    public float Speed;
    [SerializeField] float JumpForceAmount;
    [SerializeField] float JumpCooldown;
    bool canJumpCooldown = true;
    bool HoldingJump = false;
    [SerializeField] float JumpForceOff;
    [SerializeField] int HowManyJumpsTotal;
    int HowManyJumps;
    public Vector2 moveDir { get; private set; }
    public Vector2 lastMoveDir = new Vector2(1, 0);

    [Header("Gravity")]
    [SerializeField] float GravityMultiplier = 1.1f;
    public float DefualtGravity = 2;
    float MaximalGravity = 100;
    [HideInInspector] public float GravityAmount;

    [Header("Ground Check")]
    [SerializeField] float GroundCheckDis;
    [SerializeField] float GroundCheckUpAmount;
    [SerializeField] Vector2 GroundCheckBox;
    [SerializeField] LayerMask GroundLayer;
    public bool isGrounded { get; private set; }
    RaycastHit2D groundRayHit;
    [Header("Ground Check")]
    [SerializeField] PlayerDash PlayerDash;
    void Start()
    {
        
        return;
    }
    
    void Update()
    {
        GroundCheck();
        Movement();
        Gravity();
        if(isGrounded){
            HowManyJumps = HowManyJumpsTotal;
        }
        return;
    }

    bool GroundCheck(){
        isGrounded = false;
        if (groundRayHit = Physics2D.BoxCast(
            new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2 + GroundCheckUpAmount),
            GroundCheckBox * transform.localScale,
            0,
            Vector2.down,
            GroundCheckDis,
            GroundLayer
            ))
        {
            isGrounded = true;
        }

        return isGrounded;
    }

    void Gravity()
    {
        if(isGrounded) 
        {
            GravityAmount = DefualtGravity;
            return;
        }
        else{
            GravityAmount += GravityAmount * GravityMultiplier * Time.deltaTime;
            GravityAmount = Math.Clamp(GravityAmount, DefualtGravity, MaximalGravity);
            rb.AddForce(transform.up * -1 * Time.deltaTime * GravityAmount, ForceMode2D.Force);
        }
        return;
    }

    void Movement(){
        if (!PlayerDash.isDashing)
        {
            rb.linearVelocityX = moveDir.x * Speed;
            if (moveDir.x != lastMoveDir.x && moveDir.x != 0)
            {
                lastMoveDir = moveDir;
            }
        }
        return;
    }

    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        return;
    }

    void ResetJump(){
        canJumpCooldown = true;
        return;
    }

    void OnJump(){
        HoldingJump = !HoldingJump;

        if((isGrounded || (!isGrounded && HowManyJumps > 1)) && canJumpCooldown && HoldingJump){
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * JumpForceAmount, ForceMode2D.Impulse);
            canJumpCooldown = false;
            HowManyJumps--;
        }
        if(HoldingJump == false && rb.linearVelocityY >= 0 && !isGrounded){
            rb.linearVelocityY = JumpForceOff;
        }
        Invoke(nameof(ResetJump), JumpCooldown);
        return;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 startPos = new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2 + GroundCheckUpAmount);
        if (isGrounded)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(startPos, Vector2.down * groundRayHit.distance);
            Gizmos.DrawWireCube(startPos + Vector3.down * groundRayHit.distance, GroundCheckBox * transform.localScale);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(startPos, Vector2.down * GroundCheckDis);
            Gizmos.DrawWireCube(startPos + Vector3.down * GroundCheckDis, GroundCheckBox * transform.localScale);
        }
        return;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
#endif
}
