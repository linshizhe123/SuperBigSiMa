using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OttoBrick : RaycastBaseClass
{
    [Header("参数")]
    public float speed = 7.27f;
    public bool isHadCoin = true;
    public bool canMove = false;
    public bool canMoveBack = false;
    public GameObject deathBricks;
    public LayerMask playerLayer;

    private Vector3 pos;
    private Vector3 movePos;
    private Quaternion rot;

    private OttoBrick current;

    // Start is called before the first frame update
    void Start()
    {
        if (!current)
            current = this;
        pos = transform.position;
        rot = transform.rotation;
        movePos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (canMove)
            Move();
        else
        {
            if (canMoveBack)
                MoveBack();
        }
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!GameManager.BricksBrokenCheck())
                return;
            RaycastHit2D leftCheck = Raycast(new Vector2(-0.49f, -0.5f), Vector2.down, 0.4f, playerLayer);
            RaycastHit2D rightCheck = Raycast(new Vector2(0.49f, -0.5f), Vector2.down, 0.4f, playerLayer);
            RaycastHit2D midCheck = Raycast(new Vector2(0f, -0.5f), Vector2.down, 0.3f, playerLayer);

            if (leftCheck || rightCheck || midCheck)
            {
                if (isHadCoin)
                {
                    isHadCoin = false;
                    canMove = true;
                    GameObject.Find("Otto").SendMessage("OttoBrickCheck");
                }
            }
        }
    }

    // 向上移动
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, speed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, movePos) < 0.001f)
        {
            canMove = false;
            canMoveBack = true;
        }
    }

    // 移动返回
    void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, pos) < 0.001f)
        {
            canMoveBack = false;
            DeathBricksReplace();
        }
    }

    // 死砖替换
    public void DeathBricksReplace()
    {
        Instantiate(deathBricks, pos, rot);
    }
}
