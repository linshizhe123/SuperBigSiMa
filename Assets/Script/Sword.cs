using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool canMoveUp;
    public float speedUp = 7.27f;

    private Vector3 movePos;

    // Start is called before the first frame update
    void Start()
    {
        movePos = new Vector3(transform.position.x - 0.2f, transform.position.y + 2f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveUp)
            MoveUp();
    }

    public void SwordBrickCheck()
    {
        canMoveUp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.isSword = true;
        }
    }

    // 向上移动
    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, speedUp * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePos) < 0.001f)
        {
            canMoveUp = false;

            // 伸缩复原
            transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        }
    }
}
