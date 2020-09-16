using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otto : RaycastBaseClass
{
    public float speed;
    public Rigidbody2D rb;
    public CapsuleCollider2D coll;
    public LayerMask groundLayer;
    public LayerMask boundaryLayer;
    public bool canMove;
    public bool canMoveUp;
    public float speedUp = 7.27f;
    public GameObject deathOtto;


    private Vector3 movePos;
    private Otto current;
    void Start()
    {
        if (!current)
            current = this;
        rb = transform.GetComponent<Rigidbody2D>();
        coll = transform.GetComponent<CapsuleCollider2D>();

        movePos = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
    }

    private void FixedUpdate()
    {
        RaycastCheck();
        if (canMove)
            Move();
        if (canMoveUp)
            MoveUp();
    }

    public void OttoBrickCheck()
    {
        canMoveUp = true;
    }

    // 向上移动
    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, speedUp * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePos) < 0.001f)
        {
            canMoveUp = false;

            // 伸缩复原
            transform.localScale = new Vector3(1f, 1f, 1f);
            rb.bodyType = RigidbodyType2D.Dynamic;
            canMove = true;
        }
    }

    // 移动
    void Move()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
    }

    // 射线检测
    void RaycastCheck()
    {
        RaycastHit2D RightCheck = Raycast(new Vector2(0.4f, 0f), Vector2.right, 0.2f, groundLayer);
        if (RightCheck)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            speed *= -1;
        }
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(deathOtto, transform.position, transform.rotation);
            transform.gameObject.SetActive(false);
            GameManager.isHat = true;
        }

        if (collision.gameObject.tag == "Enemies")
        {
            Destroy(gameObject);
        }
    }
}
