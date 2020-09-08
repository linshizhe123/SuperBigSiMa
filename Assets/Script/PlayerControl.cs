using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private PolygonCollider2D coll;
    private Animator anim;
    private float jumpTime; // 判断长按跳跃时间

    [Header("按键控制")]
    public bool jumpPressed; // 按一次
    public bool jumpHold;  // 长按

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

    [Header("环境监测")]
    public LayerMask groundLayer;
    public LayerMask bricksLayer;
    public LayerMask coinBricksLayer;
    public float footOffset = 0.3f;
    public float groundDistance = 0.2f; // 脚下射线距离
    public float headDistance = 0.5f; // 头顶射线距离

    float xPos;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        coll = transform.GetComponent<PolygonCollider2D>();
        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
        jumpHold = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        physicsCheck();
        PlayerDirection();
        PlayerMovement();
        PlayerJump();
        PlayAnimation();
    }

    void PlayerDirection()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");
        if (xVelocity != 0)
        {
            transform.localScale = new Vector3(xVelocity, 1, 1);
            xPos = transform.position.x;
        }
        else
        {
            RaycastHit2D leftCheckBricks = Raycast(new Vector2(-footOffset, -1f), Vector2.down, groundDistance, bricksLayer);
            RaycastHit2D rightCheckBricks = Raycast(new Vector2(footOffset, -1f), Vector2.down, groundDistance, bricksLayer);
            // 不知道为什么站在高处就会滑动 只好锁定一下position
            if (leftCheckBricks || rightCheckBricks)
                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
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
            if (RaycastFeet(layer))
            {
                checkResult = true;
                break;
            }
        }
        // 左右脚射线
        //RaycastHit2D leftCheckGround = Raycast(new Vector2(-footOffset, -1f), Vector2.down, groundDistance, groundLayer);
        //RaycastHit2D rightCheckGround = Raycast(new Vector2(footOffset, -1f), Vector2.down, groundDistance, groundLayer);
        //RaycastHit2D leftCheckBricks = Raycast(new Vector2(-footOffset, -1f), Vector2.down, groundDistance, bricksLayer);
        //RaycastHit2D rightCheckBricks = Raycast(new Vector2(footOffset, -1f), Vector2.down, groundDistance, bricksLayer);
        //RaycastHit2D leftCheckCoinBricks = Raycast(new Vector2(-footOffset, -1f), Vector2.down, groundDistance, coinBricksLayer);
        //RaycastHit2D rightCheckCoinBricks = Raycast(new Vector2(footOffset, -1f), Vector2.down, groundDistance, coinBricksLayer);
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
    bool RaycastFeet(LayerMask layer)
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, -1f), Vector2.down, groundDistance, layer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -1f), Vector2.down, groundDistance, layer);
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
