using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float jumpTime; // 判断长按跳跃时间

    [Header("按键控制")]
    public bool jumpPressed; // 按一次
    public bool jumpHold;  // 长按
    public bool attackPressed;

    [Header("参数")]
    public float speed;
    public float jumpForce;
    public float jumpHoldForce;
    public float jumpHoldDuration = 0.1f;

    [Header("状态")]
    public bool isOnGround;
    public bool isJumping;
    public bool isHeadBlocked; // 撞到头
    public float xVelocity;
    public bool isAttack;

    [Header("环境监测")]
    public LayerMask groundLayer;
    public LayerMask bricksLayer;
    public LayerMask coinBricksLayer;
    public LayerMask enemiesLayer;
    public float footOffset = 0.3f;
    public float groundDistance = 0.2f; // 脚下到地面射线距离
    public float headDistance = 0.5f; // 头顶射线距离
    public float enemiesDistance = 0.3f; // 脚下到敌人射线距离
    public GameObject deathPlayer;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
        jumpHold = Input.GetButton("Jump");
        if (Input.GetKeyDown("x"))
            attackPressed = true;
        if (GameManager.isHat)
            transform.GetChild(0).gameObject.SetActive(true);
        if (GameManager.isSword)
            transform.GetChild(1).gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        physicsCheck();
        PlayerDirection();
        PlayerMovement();
        PlayerJump();
        PlayAnimation();
        TreadEnemies();
        DeathCheck();
        PlayerAttack();
    }

    void PlayerDirection()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");
        if (xVelocity != 0)
        {
            transform.localScale = new Vector3(xVelocity, 1, 1);
        }
    }

    void PlayerMovement()
    {
        rb.velocity = new Vector2(xVelocity * speed * Time.deltaTime, rb.velocity.y);
    }

    void PlayerJump()
    {
        if (jumpPressed && isOnGround && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime = Time.time + jumpHoldDuration;
            isJumping = true;
        }
        else if (isJumping)
        {
            if (jumpHold)
                rb.velocity = new Vector2(rb.velocity.x, jumpHoldForce);
            if (jumpTime < Time.time)
                isJumping = false;
        }
        jumpPressed = false;
    }

    // 角色攻击
    void PlayerAttack()
    {
        if (attackPressed)
        {
            anim.SetBool("isAttacking", true);
            attackPressed = false;
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    // 踩敌人检测
    void TreadEnemies()
    {
        if (GameManager.isTread)
        {
            rb.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
            GameManager.isTread = false;
        }
    }

    // 死亡检测
    void DeathCheck()
    {
        if (!GameManager.isLive)
        {
            Time.timeScale = 0;
            gameObject.SetActive(false);
            Instantiate(deathPlayer, transform.position, transform.rotation);
            GameManager.isLive = true;
        }
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
            GameManager.PlayerDied();
        }
    }

    // 动画参数修改
    void PlayAnimation()
    {
        anim.SetBool("isRuning", rb.velocity.x != 0);
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("IsJumping", isJumping);
    }

    // 物理检测
    void physicsCheck()
    {
        // 左右脚射线
        bool checkResult = false;
        LayerMask[] layerList = { groundLayer, bricksLayer, coinBricksLayer };
        foreach (LayerMask layer in layerList)
        {
            if (RaycastFeet(layer, groundDistance))
            {
                checkResult = true;
                break;
            }
        }
        if (checkResult)
            isOnGround = true;
        else isOnGround = false;

        // 头顶射线
        RaycastHit2D headCheck = Raycast(new Vector2(0f, 0.8f), Vector2.up, headDistance, bricksLayer);
        if (headCheck)
            isHeadBlocked = true;
        else isHeadBlocked = false;

    }

    // 封装左右脚射线遍历方法
    bool RaycastFeet(LayerMask layer, float distance)
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, -1f), Vector2.down, distance, layer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -1f), Vector2.down, distance, layer);
        return leftCheck || rightCheck;
    }

    // 射线函数封装
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.05f);
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction * length, color);
        return hit;
    }
}
