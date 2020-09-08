using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BrokenBricks : BricksBaseClass
{
    public GameObject bro1;
    public GameObject bro2;
    public GameObject bro3;
    public GameObject bro4;
    public LayerMask playerLayer;

    private Rigidbody2D bro1Rb;
    private Rigidbody2D bro2Rb;
    private Rigidbody2D bro3Rb;
    private Rigidbody2D bro4Rb;

    private BrokenBricks current;
    // Start is called before the first frame update
    void Start()
    {
        if (!current)
            current = this;
        bro1Rb = bro1.GetComponentInChildren<Rigidbody2D>();
        bro2Rb = bro2.GetComponentInChildren<Rigidbody2D>();
        bro3Rb = bro3.GetComponentInChildren<Rigidbody2D>();
        bro4Rb = bro4.GetComponentInChildren<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RaycastHit2D leftCheck = Raycast(new Vector2(-0.49f, -0.5f), Vector2.down, 0.3f, playerLayer);
            RaycastHit2D rightCheck = Raycast(new Vector2(0.49f, -0.5f), Vector2.down, 0.3f, playerLayer);
            if (leftCheck || rightCheck)
            {
                current.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0);
                Destroy(current.GetComponent<BoxCollider2D>(), 0.3f);
                Rigidbody2D[] rbLsit = { bro1Rb, bro2Rb, bro3Rb, bro4Rb };
                GameObject[] gobjLsit = { bro1, bro2, bro3, bro4 };
                foreach (GameObject gobj in gobjLsit)
                {
                    gobj.SetActive(true);
                }
                foreach (Rigidbody2D rb in rbLsit)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
                Brokening();
            }
        }
    }

    public void Brokening()
    {
        bro1Rb.velocity = new Vector2(3f, 2f);
        bro2Rb.velocity = new Vector2(-3f, 2f);
        bro3Rb.velocity = new Vector2(3f, 4f);
        bro4Rb.velocity = new Vector2(-3f, 4f);
        Destroy(current.gameObject, 2f);
    }
}
