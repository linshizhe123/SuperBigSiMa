using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZhixun : RaycastBaseClass
{
    private Rigidbody2D rb;
    private RaycastHit2D leftCheck;
    private RaycastHit2D rightCheck;
    private RaycastHit2D upLeftCheck;
    private RaycastHit2D upRightCheck;
    private RaycastHit2D upLeftLeftCheck;
    private RaycastHit2D upRightRightCheck;
    private RaycastHit2D leftPlayerCheck;
    private RaycastHit2D rightPlayerCheck;

    public LayerMask ground;
    public LayerMask player;
    private bool canTread = true;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        RaycastCheck();
        TreadCheck();
        ColumnCheck();
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }

    // 碰撞柱子检测&&角色死亡检测
    void ColumnCheck()
    {
        if (leftCheck)
            rb.velocity = new Vector2(20f, rb.velocity.y);
        if (rightCheck)
            rb.velocity = new Vector2(-20f, rb.velocity.y);

        if (isMoving && (leftPlayerCheck || rightPlayerCheck))
        {
            GameManager.isLiveOrDead(false);
        }
    }

    // Player踩踏检测
    void TreadCheck()
    {
        if (!canTread)
        {
            return;
        }
        if ((upLeftCheck || upLeftLeftCheck) && (!upRightCheck && !upRightRightCheck))
        {
            canTread = false;
            GameManager.isTreadEnemies(true);
            rb.velocity = new Vector2(20f, rb.velocity.y);
            Invoke("TreadFlag", 0.3f);
            isMoving = true;
        }
        else if ((upRightCheck || upRightRightCheck) && (!upLeftCheck && !upLeftLeftCheck))
        {
            canTread = false;
            GameManager.isTreadEnemies(true);
            rb.velocity = new Vector2(-20f, rb.velocity.y);
            Invoke("TreadFlag", 0.3f);
            isMoving = true;
        }
        else if (upLeftLeftCheck && upRightRightCheck && !upRightCheck && !upLeftCheck)
        {
            canTread = false;
            GameManager.isTreadEnemies(true);
            rb.velocity = new Vector2(20f, rb.velocity.y);
            Invoke("TreadFlag", 0.3f);
        }
    }

    void TreadFlag()
    {
        canTread = true;
    }

    // 射线检测
    void RaycastCheck()
    {
        leftCheck = Raycast(new Vector2(-0.49f, -0.1f), Vector2.left, 0.25f, ground);
        rightCheck = Raycast(new Vector2(0.87f, -0.1f), Vector2.right, 0.25f, ground);

        leftPlayerCheck = Raycast(new Vector2(-0.49f, -0.1f), Vector2.left, 0.25f, player);
        rightPlayerCheck = Raycast(new Vector2(0.87f, -0.1f), Vector2.right, 0.25f, player);

        upLeftCheck = Raycast(new Vector2(-0.52f, 0.1f), new Vector2(0.15f, 1f), 0.2f, player);
        upRightCheck = Raycast(new Vector2(0.84f, 0.1f), new Vector2(-0.15f, 1f), 0.2f, player);
        upLeftLeftCheck = Raycast(new Vector2(-0.07f, 0.1f), Vector2.up, 0.2f, player);
        upRightRightCheck = Raycast(new Vector2(0.45f, 0.1f), Vector2.up, 0.2f, player);
    }
}
