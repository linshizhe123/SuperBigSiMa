using UnityEngine;

public class CoinBricks : RaycastBaseClass
{
    [Header("参数")]
    public float speed = 7.27f;
    public bool isHadCoin = true;
    public bool canMove = false;
    public bool canMoveBack = false;
    public bool coinCanMove = false;
    public bool coinCanMoveBack = false;
    public GameObject deathBricks;
    public LayerMask playerLayer;

    [Header("金币")]
    public GameObject coin;
    public Animator coinAnim;
    private Vector3 coinPos;
    private Vector3 coinMovePos;

    private Vector3 pos;
    private Vector3 movePos;
    private Quaternion rot;

    private CoinBricks current;

    private void Start()
    {
        if (!current)
            current = this;
        pos = transform.position;
        rot = transform.rotation;
        movePos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        // 金币
        coin = transform.GetChild(0).gameObject;
        coinAnim = transform.GetComponentInChildren<Animator>();
        coinPos = coin.transform.position;
        coinMovePos = new Vector3(coinPos.x, coinPos.y + 3f, coinPos.z);
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

        // 金币
        if (coinCanMove)
            CoinMove();
        else
        {
            if (coinCanMoveBack)
                CoinMoveBack();
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
                    coinCanMove = true;
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

    // 金币移动
    void CoinMove()
    {
        coinAnim.SetBool("isBounce", true);
        coin.transform.position += new Vector3(0, 0.2f, 0);
        if (Vector3.Distance(coin.transform.position, coinMovePos) <= 0.21f)
        {
            coinCanMove = false;
            coinCanMoveBack = true;
        }
    }

    void CoinMoveBack()
    {
        coin.transform.position += new Vector3(0, -0.2f, 0);
        if (Vector3.Distance(coin.transform.position, coinPos) <= 0.21f)
        {
            coinCanMoveBack = false;
            coinAnim.SetBool("isBounce", false);
        }
    }

    // 死砖替换
    public void DeathBricksReplace()
    {
        Instantiate(deathBricks, pos, rot);
        Destroy(current.gameObject, 2f);
    }
}
