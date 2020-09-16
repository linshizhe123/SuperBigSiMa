using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pdd : RaycastBaseClass
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
    private RaycastHit2D leftDeathZhixunCheck;
    private RaycastHit2D rightDeathZhixunCheck;

    public float speed;
    public LayerMask ground;
    public LayerMask player;
    public LayerMask DeathZhixun;
    public GameObject deathPdd;

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
            DeathZhixunCheck();
        }
    }

    // 移动
    void PlayerMovement()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
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
            Instantiate(deathPdd, new Vector3(transform.position.x, -3.65f, transform.position.z), transform.rotation);
            GameManager.isTreadEnemies(true);
        }

        // 死亡检测
        if (leftPlayerCheck || rightPlayerCheck)
        {
            GameManager.isLiveOrDead(false);
        }
    }

    // 龟壳碰撞检测
    void DeathZhixunCheck()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.1 && (leftDeathZhixunCheck || rightDeathZhixunCheck))
        {
            transform.Rotate(0f, 0f, -180);
            rb.AddForce(new Vector2(0, 3f), ForceMode2D.Impulse);
            Destroy(transform.GetComponent<BoxCollider2D>());
            Destroy(gameObject, 2f);
        }
    }

    // 射线检测
    void RaycastCheck()
    {
        leftCheck = Raycast(new Vector2(-0.31f, 0.3f), Vector2.left, 0.25f, ground);
        rightCheck = Raycast(new Vector2(0.31f, 0.3f), Vector2.right, 0.25f, ground);

        upLeftCheck = Raycast(new Vector2(-0.32f, 0.45f), new Vector2(-0.1f, 1f), 0.2f, player);
        upMidCheck = Raycast(new Vector2(0f, 0.45f), Vector2.up, 0.2f, player);
        upRightCheck = Raycast(new Vector2(0.32f, 0.45f), new Vector2(0.1f, 1f), 0.2f, player);

        leftPlayerCheck = Raycast(new Vector2(-0.35f, 0.3f), Vector2.left, 0.2f, player);
        rightPlayerCheck = Raycast(new Vector2(0.35f, 0.3f), Vector2.right, 0.2f, player);

        leftDeathZhixunCheck = Raycast(new Vector2(-0.35f, 0.3f), Vector2.left, 0.2f, DeathZhixun);
        rightDeathZhixunCheck = Raycast(new Vector2(0.35f, 0.3f), Vector2.right, 0.2f, DeathZhixun);
    }

    // 在摄像机内触发函数
    private void OnBecameVisible()
    {
        startMove = true;
    }
}
