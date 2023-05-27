using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 250;
    [SerializeField] private float jumpForce = 400;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;

    private float horizontal;

    private string currentAnimName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        isGrounded = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.X) && isGrounded)
            {
                Debug.Log("X");
                Jump();
            }

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
            {
                Debug.Log("Z");
                Attack();
            }
            
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Debug.Log("C");
                Throw();
            }
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0? 0 : 180, 0));
            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        
    }

    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        return hit.collider != null;
    }

    private void Attack()
    { 
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }
    
    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
