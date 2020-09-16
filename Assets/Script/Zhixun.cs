using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zhixun : RaycastBaseClass
{
    private Rigidbody2D rb;
    private bool startMove = false;
    private RaycastHit2D leftCheck;
    private RaycastHit2D rightCheck;
    private RaycastHit2D upMidCheck;
    private RaycastHit2D upLeftCheck;
    private RaycastHit2D upRightCheck;
    private RaycastHit2D leftPlayerCheck;
    private RaycastHit2D rightPlayerCheck;

    public float speed;
    public LayerMask ground;
    public LayerMask player;
    public GameObject deathZhixun;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (startMove)
        {
            PlayerMovement();
            RaycastCheck();
            TreadCheck();
        }
    }

    // 移动
    void PlayerMovement()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
        transform.localScale = new Vector3(speed > 0 ? -1 : 1, 1, 1);
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Column")
        {
            if (leftCheck)
                speed = 200f;
            if (rightCheck)
                speed = -200f;
        }

        if (collision.gameObject.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }

    // Player踩踏检测
    void TreadCheck()
    {
        if (upLeftCheck || upMidCheck || upRightCheck)
        {
            Destroy(transform.gameObject);
            Instantiate(deathZhixun, transform.position, transform.rotation);
            GameManager.isTreadEnemies(true);
        }

        // 死亡检测
        if (leftPlayerCheck || rightPlayerCheck)
        {
            GameManager.isLiveOrDead(false);
        }
    }

    // 射线检测
    void RaycastCheck()
    {
        leftCheck = Raycast(new Vector2(-0.65f, 0.4f), Vector2.left, 0.25f, ground);
        rightCheck = Raycast(new Vector2(0.95f, 0.4f), Vector2.right, 0.25f, ground);

        upLeftCheck = Raycast(new Vector2(-0.65f, 0.7f), new Vector2(-0.1f, 1f), 0.2f, player);
        upMidCheck = Raycast(new Vector2(0.2f, 0.7f), Vector2.up, 0.2f, player);
        upRightCheck = Raycast(new Vector2(0.95f, 0.7f), new Vector2(0.1f, 1f), 0.2f, player);

        leftPlayerCheck = Raycast(new Vector2(-0.65f, 0.4f), Vector2.left, 0.2f, player);
        rightPlayerCheck = Raycast(new Vector2(0.95f, 0.4f), Vector2.right, 0.2f, player);
    }

    // 在摄像机内触发函数
    private void OnBecameVisible()
    {
        startMove = true;
    }
}
